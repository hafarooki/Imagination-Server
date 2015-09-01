#pragma once

#include "Stdafx.h"
#include <RakPeerInterface.h>
#include <iostream>
#include "WBitStream.h"
#include "WPacketPriority.h"

using namespace RakNet;
using namespace System;

public ref class BaseServer abstract
{
	RakPeerInterface *_peer;

	int _port;
	int _maxConnections;
	char* _address;

	cli::array<unsigned char>^ native_to_managed_array(unsigned char* native, unsigned int length)
	{
		cli::array<unsigned char>^ managed = gcnew cli::array<unsigned char>(length);
		for (int i = 0, i_max = managed->Length; i < i_max; ++i)
			managed[i] = native[i];
		return managed;
	}
protected:
	BaseServer(int port, int maxConnections, String^ address);

	virtual void OnStart() = 0;
	virtual void OnStop() = 0;
	virtual void OnReceived(cli::array<unsigned char>^ bytes, unsigned int length, String^ address) = 0;
	virtual void OnDisconnect(String^ address) = 0;
	virtual void OnConnect(String^ address) = 0;

public:
	void SendInitPacket(bool auth, String^ address);
	void Start();
	void Stop();

	void Send(WBitStream^ bitStream, WPacketPriority priority, WPacketReliability reliability, char orderingChannel, String^ systemAddress, bool broadcast);
};

struct InitPacket {
	unsigned long versionId; // The game version
	unsigned long unknown1; // Dunno what this is...
	unsigned long remoteConnectionType; // This the remote connection type (1 = auth, 4 = other)
	unsigned long processId; // The process ID of the server
	unsigned short unknown2; // Dunno what this is either... it is "FF FF" in hex
	std::string ipString; // The IP string of the server (will be changed programmatically)

	InitPacket() {}

	InitPacket(bool isAuth) {
		// Set the variables
		versionId = 171022;
		unknown1 = 147;
		remoteConnectionType = isAuth ? 1 : 4; // Make sure to set this!!!! Determines whether remoteConnectionType should be 1 or 4
		processId = 5136;
		unknown2 = 65535;
		ipString = "127.0.0.1";
	}
};