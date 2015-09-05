// This is the main DLL file.

#include "stdafx.h"

#include "ImaginationServer.World.Packets.h"
#include <RakPeerInterface.h>
#include <iostream>

using namespace ImaginationServer::Common;

void ImaginationServerWorldPackets::WorldPackets::SendCharacterListResponse(String ^ address, unsigned char characters)
{
	BitStream bitStream;
	CreatePacketHeader(ID_USER_PACKET_ENUM, 5, (unsigned long)PacketEnums::WorldServerPacketId::MsgClientCharacterListResponse, &bitStream);
	bitStream.Write(characters);
	//bitStream.Write((unsigned char)0);
	auto server = LuServer::CurrentServer->GetPeer();
	auto split = address->Split(':');
	SystemAddress unmanaged;
	unmanaged.SetBinaryAddress((char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(split[0])).ToPointer());
	unmanaged.port = (unsigned short)std::strtoul((char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(split[1])).ToPointer(), NULL, 0);
	server->Send(&bitStream, SYSTEM_PRIORITY, RELIABLE_ORDERED, 0, unmanaged, false);
}
