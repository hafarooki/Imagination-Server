#pragma once
#include "PacketData.h"
#include "User.h"
#include <WinSock2.h>
#include "Packets.h"

struct LoginStatusPacket : PacketData
{
	unsigned char LoginStatus; // This is the Connection ID
	string TalkLikeAPirate; // Always is "Talk_Like_A_Pirate", yet 33 bytes long
	string UnknownString; // Have NO idea what this is... Is it relevant?

	unsigned short ClientVersion1; // For some reason, the client version
	unsigned short ClientVersion2; // is split up over 3 unsigned shorts
	unsigned short ClientVersion3; // I believe...

	string Unknown; // This is 237 bytes long...

	string UserKey; // Generated User Key - Should be wstring
	string RedirectIp; // Redirect IP Address
	string ChatIp; // Chat IP Address
	unsigned short RedirectPort; // Redirect Port
	unsigned short ChatPort; // Chat Port
	string AnotherIp; // Another IP Address? 33 bytes long
	string PossibleGuid; // In the form of a UUID (maybe a GUID?)
	unsigned short ZeroShort; // Always 0 for some reason...
	unsigned char LocalizationChar[3]; // Can be IT, US, etc... Always seems to end in 0
	unsigned char FirstLoginSubscription; // Check - Is this the first login after subscribing? 
	unsigned char Subscribed; // 0 if subscribed, 1 if not subscribed (for LUNI, always 0)
	unsigned long long ZeroLong; // This is all zeros...
	unsigned short ErrorMsgLength; // This is the length of the error msg
	string ErrorMsg; // This is the error message displayed if connection ID is 0x05. Can be X bytes long (I believe) - Should be wstring

					 // Extra bytes
	unsigned short ExtraBytesLength; // This is the number of bytes left (number of each chunk of extra data = 16 bytes * x chunks + these 4 bytes

									 // Initializer
	explicit LoginStatusPacket(UserSuccess loginStatus)
	{
		struct in_addr addr;
		char ac[80];
		gethostname(ac, sizeof(ac));
		auto phe = gethostbyname(ac);
		memcpy(&addr, phe->h_addr_list[0], sizeof(struct in_addr));
		LoginStatus = loginStatus;
		TalkLikeAPirate = "Talk_Like_A_Pirate";
		UnknownString = "";
		ClientVersion1 = 1;
		ClientVersion2 = 10;
		ClientVersion3 = 64;
		Unknown = "_";
		UserKey = "0 9 4 e 7 0 1 a c 3 b 5 5 2 0 b 4 7 8 9 5 b 3 1 8 5 7 b f 1 c 3   ";
		ChatIp = inet_ntoa(addr);
		ChatPort = 2006;
		AnotherIp = inet_ntoa(addr);
		PossibleGuid = "00000000 - 0000 - 0000 - 0000 - 000000000000";
		ZeroShort = 0;
		LocalizationChar[0] = 0x55;
		LocalizationChar[1] = 0x53;
		LocalizationChar[2] = 0x00;
		FirstLoginSubscription = 0;
		Subscribed = 0;
		ZeroLong = 0;
		RedirectIp = inet_ntoa(addr);
		RedirectPort = 2006;
		ErrorMsg = "T";
		ErrorMsgLength = 0;
		ExtraBytesLength = 324;
	}

	virtual void Serialize(BitStream* output) override
	{
		output->Write(LoginStatus);
		WriteStringToBitStream(TalkLikeAPirate.c_str(), 18, 33, output);
		for (auto i = 0; i < 7; i++)
		{
			WriteStringToBitStream(UnknownString.c_str(), 0, 33, output);
		}
		output->Write(ClientVersion1);
		output->Write(ClientVersion2);
		output->Write(ClientVersion3); 
		WriteStringToBitStream(UserKey.c_str(), 66, 66, output);
		WriteStringToBitStream(RedirectIp.c_str(), sizeof(RedirectIp), 33, output);
		WriteStringToBitStream(ChatIp.c_str(), sizeof(ChatIp), 33, output);
		output->Write(RedirectPort);
		output->Write(ChatPort);
		WriteStringToBitStream(AnotherIp.c_str(), sizeof(AnotherIp), 33, output);
		WriteStringToBitStream(PossibleGuid.c_str(), 37, 37, output);
		output->Write(ZeroShort);
		output->Write(LocalizationChar);
		output->Write(FirstLoginSubscription);
		output->Write(Subscribed);
		output->Write(ZeroLong);
		output->Write(ErrorMsgLength);
		WriteStringToBitStream(ErrorMsg.c_str(), ErrorMsgLength, 1, output);
		output->Write(ExtraBytesLength);
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