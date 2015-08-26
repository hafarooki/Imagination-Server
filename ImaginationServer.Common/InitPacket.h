#pragma once
#include <string>
#include "PacketData.h"
#include <WinSock2.h>

struct InitPacket : PacketData
{
	unsigned long VersionId; // The game version
	unsigned long Unknown1; // Dunno what this is...
	unsigned long RemoteConnectionType; // This the remote connection type (1 = auth, 4 = other)
	unsigned long ProcessId; // The process ID of the server
	unsigned short Unknown2; // Dunno what this is either... it is "FF FF" in hex
	string IpString; // The IP string of the server (will be changed programmatically)

	explicit InitPacket(bool isAuth)
	{
		VersionId = 171022;
		Unknown1 = 147;
		RemoteConnectionType = isAuth ? 1 : 4; // Make sure to set this!!!! Determines whether remoteConnectionType should be 1 or 4 
		ProcessId = 5136;
		Unknown2 = 65535;		
		struct in_addr addr;
		char ac[80];
		gethostname(ac, sizeof(ac));
		auto phe = gethostbyname(ac);
		memcpy(&addr, phe->h_addr_list[0], sizeof(struct in_addr));
		IpString = inet_ntoa(addr);
	}

	virtual void Serialize(BitStream* output) override
	{
		output->Write(reinterpret_cast<char*>(this), sizeof(this));
	}
};
