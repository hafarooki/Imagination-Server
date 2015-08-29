#pragma once
#include <string>
#include "PacketData.h"
#include <WinSock2.h>
//
//struct InitPacket : PacketData
//{
//	unsigned long VersionId; // The game version
//	unsigned long Unknown1; // Dunno what this is...
//	unsigned long RemoteConnectionType; // This the remote connection type (1 = auth, 4 = other)
//	unsigned long ProcessId; // The process ID of the server
//	unsigned short Unknown2; // Dunno what this is either... it is "FF FF" in hex
//	string IpString; // The IP string of the server (will be changed programmatically)
//
//	explicit InitPacket(bool isAuth)
//	{
//		VersionId = 171022;
//		Unknown1 = 147;
//		RemoteConnectionType = isAuth ? 1 : 4; // Make sure to set this!!!! Determines whether remoteConnectionType should be 1 or 4 
//		ProcessId = 5136;
//		Unknown2 = 65535;
//		IpString = "127.0.0.1";
//	}
//
//	virtual void Serialize(BitStream* output) override
//	{
//		output->Write((char*)this, sizeof(this));
//	}
//};

struct InitPacket {
	unsigned long versionId; // The game version
	unsigned long unknown1; // Dunno what this is...
	unsigned long remoteConnectionType; // This the remote connection type (1 = auth, 4 = other)
	unsigned long processId; // The process ID of the server
	unsigned short unknown2; // Dunno what this is either... it is "FF FF" in hex
	string ipString; // The IP string of the server (will be changed programmatically)
	
	explicit InitPacket(char* address, bool isAuth) {
		// Set the variables
		versionId = 171022;
		unknown1 = 147;
		remoteConnectionType = isAuth ? 1 : 4; // Make sure to set this!!!! Determines whether remoteConnectionType should be 1 or 4
		processId = 5136;
		unknown2 = 65535;
		ipString = address;
	}
};

class HandshakePacket : public PacketData
{
	bool isAuth;
	char* address;

public:
	explicit HandshakePacket(char* addr, bool auth)
	{
		isAuth = auth;
		address = addr;
	}

	virtual void Serialize(BitStream* output) override
	{
		InitPacket initPacket(address, isAuth);
		output->Write((char*)&initPacket, sizeof(initPacket));
	}
};