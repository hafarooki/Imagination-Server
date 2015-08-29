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

	template<typename T, size_t N>
	cli::array<unsigned char>^ native_to_managed_array(T const (&native)[N])
	{
		cli::array<unsigned char>^ managed = gcnew cli::array<unsigned char>(N);
		for (int i = 0, i_max = managed->Length; i < i_max; ++i)
			managed[i] = native[i];
		return managed;
	}
protected:
	BaseServer(int port, int maxConnections, String^ address);

	virtual void OnStart() = 0;
	virtual void OnStop() = 0;
	virtual void OnReceived(cli::array<unsigned char>^ data, String^ address) = 0;
	virtual void OnDisconnect(String^ address) = 0;
	virtual void OnConnect(String^ address) = 0;
public:
	void Start();
	void Stop();

	void Send(WBitStream^ bitStream, WPacketPriority priority, WPacketReliability reliability, char orderingChannel, String^ systemAddress, bool broadcast);
};