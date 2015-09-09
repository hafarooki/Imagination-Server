#include "Stdafx.h"

#include <RakNetworkFactory.h>
#include <MessageIdentifiers.h>
#include <RSACrypt.h>
#include <BigTypes.h>

#include "BaseServer.h"

using namespace System::Runtime::InteropServices;
using namespace System;
using namespace big;

BaseServer::BaseServer(int port, int maxConnections, String^ address)
{
	_port = port;
	_maxConnections = maxConnections;
	_address = (char*)(Marshal::StringToHGlobalAnsi(address)).ToPointer();
}

void BaseServer::Send(WBitStream^ bitStream, WPacketPriority priority, WPacketReliability reliability, char orderingChannel, String^ systemAddress, bool broadcast)
{	
	auto split = systemAddress->Split(':');
	auto unmanaged = new SystemAddress();
	unmanaged->SetBinaryAddress((char*)(Marshal::StringToHGlobalAnsi(split[0])).ToPointer());
	unmanaged->port = (unsigned short) strtoul((char*)(Marshal::StringToHGlobalAnsi(split[1])).ToPointer(), NULL, 0);
	_peer->Send(bitStream->Instance, static_cast<PacketPriority>(priority), static_cast<PacketReliability>(reliability), orderingChannel, *unmanaged, broadcast);
	delete unmanaged;
}

RakPeerInterface* BaseServer::GetPeer()
{
	return _peer;
}

void BaseServer::Start()
{
	_peer = RakNetworkFactory::GetRakPeerInterface();
	
	// Initialize Security
	_peer->SetIncomingPassword("3.25 ND1", 8);
	big::u32 e; RSA_BIT_SIZE n;
	BIGHALFSIZE(RSA_BIT_SIZE, p); BIGHALFSIZE(RSA_BIT_SIZE, q);
	big::RSACrypt<RSA_BIT_SIZE> rsaCrypt;
	rsaCrypt.generateKeys();
	rsaCrypt.getPublicKey(e, n);
	rsaCrypt.getPrivateKey(p, q);
	_peer->InitializeSecurity(0, 0, (char*)p, (char*)q);

	SocketDescriptor socketDescriptor = SocketDescriptor(_port, _address);
	_peer->Startup(_maxConnections, 30, &socketDescriptor, 1);
	_peer->SetMaximumIncomingConnections(_maxConnections);
	OnStart();
	Packet *packet;

	RM = gcnew LuReplicaManager();
	NetworkIdManager = gcnew LuNetworkIdManager();

	_peer->AttachPlugin(RM->Instance);
	_peer->SetNetworkIDManager(NetworkIdManager->Instance);

	RM->Instance->SetAutoParticipateNewConnections(true);
	RM->Instance->SetAutoSerializeInScope(true);
	NetworkIdManager->Instance->SetIsNetworkIDAuthority(true);

	_terminate = false;

	while(!_terminate)
	{
		if (Environment::HasShutdownStarted)
		{
			Stop();
			break;
		}
		packet = _peer->Receive();
		if (!packet) continue;
		switch(packet->data[0])
		{
		case ID_USER_PACKET_ENUM:
			OnReceived(native_to_managed_array(packet->data, packet->length), packet->length, gcnew String(packet->systemAddress.ToString()));
			break;
		case ID_DISCONNECTION_NOTIFICATION:
		case ID_CONNECTION_LOST:
			OnDisconnect(gcnew String(packet->systemAddress.ToString()));
			break;
		case ID_NEW_INCOMING_CONNECTION:
			OnConnect(gcnew String(packet->systemAddress.ToString()));
			break;
		default:
			Console::WriteLine("Unknown RakNet packet received! {0}", packet->data[0]);
			break;
		}
	}
}

void BaseServer::SendInitPacket(bool auth, String^ address)
{
	auto split = address->Split(':');
	InitPacket initPacket(auth);
	auto unmanaged = new SystemAddress();
	unmanaged->SetBinaryAddress((char*)(Marshal::StringToHGlobalAnsi(split[0])).ToPointer());
	unmanaged->port = (unsigned short)strtoul((char*)(Marshal::StringToHGlobalAnsi(split[1])).ToPointer(), NULL, 0);
	BitStream bitStream;
	bitStream.Write((unsigned char)83);
	bitStream.Write((unsigned short)0);
	bitStream.Write((unsigned long)0);
	bitStream.Write((unsigned char)0);
	bitStream.Write((char*)&initPacket, sizeof(initPacket));
	_peer->Send(&bitStream, SYSTEM_PRIORITY, RELIABLE_ORDERED, 0, *unmanaged, false);
	delete unmanaged;
}

void BaseServer::Stop()
{
	_terminate = true;

	OnStop();
	RakNetworkFactory::DestroyRakPeerInterface(_peer);
	delete RM;
}