// This is the main DLL file.

#include "stdafx.h"

#include "ImaginationServer.Auth.Packets.h"
#include "LoginResponse.h"
#include <RakPeerInterface.h>
#include <BitStream.h>
#include <RakNetworkFactory.h>
#include <MessageIdentifiers.h>

using namespace ImaginationServer::Common;
using namespace RakNet;

void ImaginationServerAuthPackets::AuthPackets::SendLoginResponse(String ^ address, unsigned char success, String^ userKey, LuServer^ server)
{
	BitStream bitStream;
	CreatePacketHeader(ID_USER_PACKET_ENUM, 5, 0, &bitStream);
	LoginResponse response(success, userKey);
	bitStream.Write(response.LoginStatus);
	WriteStringToBitStream(response.TalkLikeAPirate.c_str(), 18, 33, &bitStream);
	for (int i = 0; i < 7; i++) {
		WriteStringToBitStream(response.UnknownString.c_str(), 0, 33, &bitStream);
	}
	bitStream.Write(response.ClientVersion1);
	bitStream.Write(response.ClientVersion2);
	bitStream.Write(response.ClientVersion3);
	WriteStringToBitStream(response.UserKey.c_str(), 66, 66, &bitStream);
	WriteStringToBitStream(response.RedirectIp.c_str(), sizeof(response.RedirectIp), 33, &bitStream);
	WriteStringToBitStream(response.ChatIp.c_str(), sizeof(response.ChatIp), 33, &bitStream);
	bitStream.Write(response.RedirectPort);
	bitStream.Write(response.ChatPort);
	WriteStringToBitStream(response.AnotherIp.c_str(), sizeof(response.AnotherIp), 33, &bitStream);
	WriteStringToBitStream(response.PossibleGuid.c_str(), 37, 37, &bitStream);
	bitStream.Write(response.ZeroShort);
	bitStream.Write(response.LocalizationChar);
	bitStream.Write(response.FirstLoginSubscription);
	bitStream.Write(response.Subscribed);
	bitStream.Write(response.ZeroLong);
	bitStream.Write(response.ErrorMsgLength);
	WriteStringToBitStream(response.ErrorMsg.c_str(), response.ErrorMsgLength, 1, &bitStream);
	bitStream.Write(response.ExtraBytesLength);

	CreateExtraPacketData(0, 0, 2803442767, &bitStream);
	CreateExtraPacketData(7, 37381, 2803442767, &bitStream);
	CreateExtraPacketData(8, 6, 2803442767, &bitStream);
	CreateExtraPacketData(9, 0, 2803442767, &bitStream);
	CreateExtraPacketData(10, 0, 2803442767, &bitStream);
	CreateExtraPacketData(11, 1, 2803442767, &bitStream);
	CreateExtraPacketData(14, 1, 2803442767, &bitStream);
	CreateExtraPacketData(15, 0, 2803442767, &bitStream);
	CreateExtraPacketData(17, 1, 2803442767, &bitStream);
	CreateExtraPacketData(5, 0, 2803442767, &bitStream);
	CreateExtraPacketData(6, 1, 2803442767, &bitStream);
	CreateExtraPacketData(20, 1, 2803442767, &bitStream);
	CreateExtraPacketData(19, 30854, 2803442767, &bitStream);
	CreateExtraPacketData(21, 0, 2803442767, &bitStream);
	CreateExtraPacketData(22, 0, 2803442767, &bitStream);
	CreateExtraPacketData(23, 4114, 2803442767, &bitStream);
	CreateExtraPacketData(27, 4114, 2803442767, &bitStream);
	CreateExtraPacketData(28, 1, 2803442767, &bitStream);
	CreateExtraPacketData(29, 0, 2803442767, &bitStream);
	CreateExtraPacketData(30, 30854, 2803442767, &bitStream);

	auto split = address->Split(':');
	SystemAddress unmanaged;
	unmanaged.SetBinaryAddress((char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(split[0])).ToPointer());
	unmanaged.port = (unsigned short)strtoul((char*)(System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(split[1])).ToPointer(), NULL, 0);
	server->GetPeer()->Send(&bitStream, SYSTEM_PRIORITY, RELIABLE_ORDERED, 0, unmanaged, false);
}

