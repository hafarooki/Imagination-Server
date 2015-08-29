#pragma once
#include "PacketData.h"
#include "User.h"
#include <WinSock2.h>
#include "Packets.h"

struct LoginStatusPacket : PacketData
{
	unsigned char loginStatus; // This is the Connection ID
	string talkLikeAPirate; // Always is "Talk_Like_A_Pirate", yet 33 bytes long
	string unknownString; // Have NO idea what this is... Is it relevant?

	unsigned short clientVersion1; // For some reason, the client version
	unsigned short clientVersion2; // is split up over 3 unsigned shorts
	unsigned short clientVersion3; // I believe...

	string unknown; // This is 237 bytes long...

	string userKey; // Generated User Key - Should be wstring
	string redirectIp; // Redirect IP Address
	string chatIp; // Chat IP Address
	unsigned short redirectPort; // Redirect Port
	unsigned short chatPort; // Chat Port
	string anotherIp; // Another IP Address? 33 bytes long
	string possibleGuid; // In the form of a UUID (maybe a GUID?)
	unsigned short zeroShort; // Always 0 for some reason...
	unsigned char localizationChar[3]; // Can be IT, US, etc... Always seems to end in 0
	unsigned char firstLoginSubscription; // Check - Is this the first login after subscribing? 
	unsigned char subscribed; // 0 if subscribed, 1 if not subscribed (for LUNI, always 0)
	unsigned long long zeroLong; // This is all zeros...
	unsigned short errorMsgLength; // This is the length of the error msg
	string errorMsg; // This is the error message displayed if connection ID is 0x05. Can be X bytes long (I believe) - Should be wstring

					 // Extra bytes
	unsigned short extraBytesLength; // This is the number of bytes left (number of each chunk of extra data = 16 bytes * x chunks + these 4 bytes

									 // Initializer
	explicit LoginStatusPacket(char* ipAddress, UserSuccess loginStatus)
	{
		this->loginStatus = loginStatus;
		talkLikeAPirate = "Talk_Like_A_Pirate";
		unknownString = "";
		clientVersion1 = 1;
		clientVersion2 = 10;
		clientVersion3 = 64;
		unknown = "_";
		userKey = "0 9 4 e 7 0 1 a c 3 b 5 5 2 0 b 4 7 8 9 5 b 3 1 8 5 7 b f 1 c 3   ";
		chatIp = ipAddress;
		chatPort = 2006;
		anotherIp = ipAddress;
		possibleGuid = "00000000 - 0000 - 0000 - 0000 - 000000000000";
		zeroShort = 0;
		localizationChar[0] = 0x55;
		localizationChar[1] = 0x53;
		localizationChar[2] = 0x00;
		firstLoginSubscription = 0;
		subscribed = 0;
		zeroLong = 0;
		redirectIp = ipAddress;
		redirectPort = 2003;
		errorMsg = "T";
		errorMsgLength = 0;
		extraBytesLength = 324;
	}

	virtual void Serialize(BitStream* output) override
	{
		output->Write(loginStatus);
		WriteStringToBitStream(talkLikeAPirate.c_str(), 18, 33, output);
		for (auto i = 0; i < 7; i++)
		{
			WriteStringToBitStream(unknownString.c_str(), 0, 33, output);
		}
		output->Write(clientVersion1);
		output->Write(clientVersion2);
		output->Write(clientVersion3); 
		WriteStringToBitStream(userKey.c_str(), 66, 66, output);
		WriteStringToBitStream(redirectIp.c_str(), sizeof(redirectIp), 33, output);
		WriteStringToBitStream(chatIp.c_str(), sizeof(chatIp), 33, output);
		output->Write(redirectPort);
		output->Write(chatPort);
		WriteStringToBitStream(anotherIp.c_str(), sizeof(anotherIp), 33, output);
		WriteStringToBitStream(possibleGuid.c_str(), 37, 37, output);
		output->Write(zeroShort);
		output->Write(localizationChar);
		output->Write(firstLoginSubscription);
		output->Write(subscribed);
		output->Write(zeroLong);
		output->Write(errorMsgLength);
		WriteStringToBitStream(errorMsg.c_str(), errorMsgLength, 1, output);
		output->Write(extraBytesLength);
		CreateExtraPacketData(0, 0, 2803442767, output);
		CreateExtraPacketData(7, 37381, 2803442767, output);
		CreateExtraPacketData(8, 6, 2803442767, output);
		CreateExtraPacketData(9, 0, 2803442767, output);
		CreateExtraPacketData(10, 0, 2803442767, output);
		CreateExtraPacketData(11, 1, 2803442767, output);
		CreateExtraPacketData(14, 1, 2803442767, output);
		CreateExtraPacketData(15, 0, 2803442767, output);
		CreateExtraPacketData(17, 1, 2803442767, output);
		CreateExtraPacketData(5, 0, 2803442767, output);
		CreateExtraPacketData(6, 1, 2803442767, output);
		CreateExtraPacketData(20, 1, 2803442767, output);
		CreateExtraPacketData(19, 30854, 2803442767, output);
		CreateExtraPacketData(21, 0, 2803442767, output);
		CreateExtraPacketData(22, 0, 2803442767, output);
		CreateExtraPacketData(23, 4114, 2803442767, output);
		CreateExtraPacketData(27, 4114, 2803442767, output);
		CreateExtraPacketData(28, 1, 2803442767, output);
		CreateExtraPacketData(29, 0, 2803442767, output);
		CreateExtraPacketData(30, 30854, 2803442767, output);
	}

	void CreateExtraPacketData(unsigned long stampId, signed long bracketNum, unsigned long afterNum, BitStream* bitStream) const
	{
		unsigned long zeroPacket = 0;

		bitStream->Write(stampId);
		bitStream->Write(bracketNum);
		bitStream->Write(afterNum);
		bitStream->Write(zeroPacket);
	}
};