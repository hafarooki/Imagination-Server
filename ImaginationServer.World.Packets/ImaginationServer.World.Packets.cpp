// This is the main DLL file.

#include "stdafx.h"

#include "ImaginationServer.World.Packets.h"
#include <RakPeerInterface.h>
#include <iostream>

using namespace ImaginationServer::Common;
using namespace ImaginationServer::Common::Data;
using namespace System::Runtime::InteropServices;

void ImaginationServerWorldPackets::WorldPackets::SendCharacterListResponse(String ^ address, Account^ account)
{
	BitStream bitStream;
	CreatePacketHeader(ID_USER_PACKET_ENUM, 5, (unsigned long)PacketEnums::WorldServerPacketId::MsgClientCharacterListResponse, &bitStream);
	bitStream.Write(account->Characters->Count);
	bitStream.Write((unsigned char)0);
	for each(auto name in account->Characters)
	{
		auto character = LuServer::CurrentServer->CacheClient->Get<Character^>("characters:" + name->ToLower());
		bitStream.Write(character->Id);
		bitStream.Write((unsigned long)0);
		std::string username = (char*)(Marshal::StringToHGlobalAnsi(character->Minifig->Name)).ToPointer();
		WriteStringToBitStream(username.c_str(), sizeof(username), 66, &bitStream);
		std::string weirdname = "Derp?";
		WriteStringToBitStream(weirdname.c_str(), sizeof(weirdname), 66, &bitStream);
		bitStream.Write((unsigned char)0);
		bitStream.Write((unsigned char)0);
		for (int i = 0; i < 10; i++) bitStream.Write((unsigned char)0);
		bitStream.Write(character->Minifig->ShirtColor);
		bitStream.Write((unsigned long)0);
		bitStream.Write(character->Minifig->PantsColor);
		bitStream.Write(character->Minifig->HairStyle);
		bitStream.Write(character->Minifig->HairColor);
		bitStream.Write(character->Minifig->Lh);
		bitStream.Write(character->Minifig->Rh);
		bitStream.Write(character->Minifig->Eyebrows);
		bitStream.Write(character->Minifig->Eyes);
		bitStream.Write(character->Minifig->Mouth);
		bitStream.Write((unsigned long)0);
		bitStream.Write(character->ZoneId);
		bitStream.Write(character->MapInstance);
		bitStream.Write((unsigned long)0);
		bitStream.Write((unsigned long long)0);
		// TODO: Send the user items
		bitStream.Write((unsigned short)0); // 0 items equipped.
	}

	auto server = LuServer::CurrentServer->GetPeer();
	auto split = address->Split(':');
	SystemAddress unmanaged;
	unmanaged.SetBinaryAddress((char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(split[0])).ToPointer());
	unmanaged.port = (unsigned short)std::strtoul((char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(split[1])).ToPointer(), NULL, 0);
	server->Send(&bitStream, SYSTEM_PRIORITY, RELIABLE_ORDERED, 0, unmanaged, false);
}
