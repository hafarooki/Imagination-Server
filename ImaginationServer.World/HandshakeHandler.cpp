#include "HandshakeHandler.h"
#include <iostream>
#include <InitPacket.h>
#include <MessageIdentifiers.h>

using namespace std;

HandshakeHandler::HandshakeHandler(char* address)
{
	Address = address;
}

void HandshakeHandler::Handle(Packet *packet, RakPeerInterface *peer)
{
	BitStream bitStream;
	CreatePacketHeader(ID_USER_PACKET_ENUM, 0, 0, &bitStream);
	auto handshakePacket = HandshakePacket(Address, false);
	//bitStream.Write((char*)&charInitPacket, sizeof(charInitPacket));
	handshakePacket.Serialize(&bitStream);
	peer->Send(&bitStream, SYSTEM_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);
}