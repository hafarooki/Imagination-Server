#pragma once

#include <Handler.h>
#include <Packets.h>

class AuthHandler : Handler
{
public:
	virtual const unsigned char GetType() { return AUTH; }
	virtual const unsigned char GetCode() { return LOGIN_REQUEST; }

	virtual void Handle(Packet *packet, RakPeerInterface *peer) {
		BitStream bitStream;
		CreatePacketHeader(ID_USER_PACKET_ENUM, 5, 0, &bitStream);
		LoginStatusPacket loginStatusPacket;
		loginStatusPacket.loginStatus = loginStatus;
		loginStatusPacket.talkLikeAPirate = "Talk_Like_A_Pirate";
		loginStatusPacket.unknownString = "";
		loginStatusPacket.clientVersion1 = 1;
		loginStatusPacket.clientVersion2 = 10;
		loginStatusPacket.clientVersion3 = 64;
		loginStatusPacket.unknown = "_";
		loginStatusPacket.userKey = "0 9 4 e 7 0 1 a c 3 b 5 5 2 0 b 4 7 8 9 5 b 3 1 8 5 7 b f 1 c 3   ";
		loginStatusPacket.chatIp = "localhost";
		loginStatusPacket.chatPort = 2006;
		loginStatusPacket.anotherIp = "localhost";
		loginStatusPacket.possibleGuid = "00000000-0000-0000-0000-000000000000";
		loginStatusPacket.zeroShort = 0;
		loginStatusPacket.localizationChar[0] = 0x55;
		loginStatusPacket.localizationChar[1] = 0x53;
		loginStatusPacket.localizationChar[2] = 0x00;
		loginStatusPacket.firstLoginSubscription = 0;
		loginStatusPacket.subscribed = 0;
		loginStatusPacket.zeroLong = 0;
		loginStatusPacket.redirectIp = "127.0.0.1";
		loginStatusPacket.redirectPort = 2006;
		loginStatusPacket.ErrorMsg = "T";
		loginStatusPacket.errorMsgLength = 0;
		loginStatusPacket.ExtraBytesLength = 324;
		bitStream.Write(loginStatusPacket.loginStatus);
	}
};