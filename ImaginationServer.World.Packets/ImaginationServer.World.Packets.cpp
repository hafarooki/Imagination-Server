// This is the main DLL file.

#include "stdafx.h"

#include "ImaginationServer.World.Packets.h"
#include <RakPeerInterface.h>
#include <iostream>
#include <string>

using namespace ImaginationServer::Common;
using namespace ImaginationServer::Common::Data;
using namespace System::Runtime::InteropServices;

void ImaginationServerWorldPackets::WorldPackets::SendCharacterListResponse(String ^ address, Account^ account)
{
	BitStream bitStream;
	CreatePacketHeader(ID_USER_PACKET_ENUM, 5, (unsigned long)PacketEnums::WorldServerPacketId::MsgClientCharacterListResponse, &bitStream);
	bitStream.Write((unsigned char)account->Characters->Count); // Number of characters (00 - 04), u8
	bitStream.Write((unsigned char)0); // Index of character in the front, u8

	int charId = 0;

	for each(auto name in account->Characters)
	{
		auto character = LuServer::CurrentServer->CacheClient->Get<Character^>("characters:" + name->ToLower());
		bitStream.Write(character->Id); // Character ID (=ChildID from replica packets = objID in xml data from chardata)
		bitStream.Write((unsigned long)0); // ???
		//std::string username = (char*)(Marshal::StringToHGlobalAnsi(character->Minifig->Name)).ToPointer();
		std::wstring username(L"");
		for each(auto chr in character->Minifig->Name->ToCharArray())
		{
			username.push_back(chr);
		}
		username.resize(66);
		bitStream.Write((char*)username.data(), sizeof(wchar_t) * username.size() / 2); // Name of character, wstring
		std::wstring weirdname(L" Player" + std::to_wstring(charId));
		weirdname.resize(66);
		bitStream.Write((char*)weirdname.data(), sizeof(wchar_t) * weirdname.size() / 2); // Name that shows up in parentheses in the client (probably for not yet approved custom names?)
		bitStream.Write((unsigned char)0); // is rejected, bool
		bitStream.Write((unsigned char)0); // is FTP, if this set but the account FreeToPlay flag in the login response packet isn’t, it asks whether you’d like to change your FreeToPlay name to a custom name, bool
		unsigned char zeroChar = 0;
		unsigned char oneChar = 1; 
 		for (int i = 0; i < 10; i++) { 
 			if (i == 1 && charId == 0) { 
 				bitStream.Write(oneChar); 
 			} 
 			else { 
 				bitStream.Write(zeroChar); 
 			} 
 		} 
		bitStream.Write(character->Minifig->ShirtColor); // Shirt color
		bitStream.Write(character->Minifig->ShirtStyle); // Shirt style
		bitStream.Write(character->Minifig->PantsColor); // Pants color
		bitStream.Write(character->Minifig->HairStyle); // Hair style
		bitStream.Write(character->Minifig->HairColor); // Hair color
		bitStream.Write(character->Minifig->Lh); // “lh”, see “<mf />” row in the xml data from chardata packet (no idea what it is)
		bitStream.Write(character->Minifig->Rh); // “rh”, see “<mf />” row in the xml data from chardata packet (no idea what it is)
		//bitStream.Write((unsigned long)0); // tmp
		//bitStream.Write((unsigned long)0); // tmp
		bitStream.Write(character->Minifig->Eyebrows); // Eyebrows
		bitStream.Write(character->Minifig->Eyes); // Eyes
		bitStream.Write(character->Minifig->Mouth); // Mouth
		bitStream.Write((unsigned long)0); // ???
		bitStream.Write(character->ZoneId); // last zone ID
		bitStream.Write(character->MapInstance); // (very likely) map instance
		bitStream.Write((unsigned long)0); // map clone (name from [53-05-00-02] structure)
		bitStream.Write((unsigned long long)0); // last login or logout timestamp of character in seconds? (xml is “llog” so both could be possible)
		bitStream.Write((unsigned short)1); // number of items to follow

		// equipped item LOTs (order of items doesn’t matter? I think it reads them in order so if we accidentally put 2 shirts the second one will be the one shown.)
		bitStream.Write(FindCharShirtID(character->Minifig->ShirtColor, character->Minifig->ShirtStyle));

		charId++;
	}

	auto server = LuServer::CurrentServer->GetPeer();
	auto split = address->Split(':');
	SystemAddress unmanaged;
	unmanaged.SetBinaryAddress((char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(split[0])).ToPointer());
	unmanaged.port = (unsigned short)std::strtoul((char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(split[1])).ToPointer(), NULL, 0);
	server->Send(&bitStream, SYSTEM_PRIORITY, RELIABLE_ORDERED, 0, unmanaged, false);
}
