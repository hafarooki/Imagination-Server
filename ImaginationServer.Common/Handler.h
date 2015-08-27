#pragma once

#include <RakPeerInterface.h>

public ref class Handler
{
public:
	virtual ~Handler()
	{
	}

	virtual unsigned char GetType() { return -1; }
	virtual unsigned char GetCode() { return -1; }

	virtual void Handle(Packet* packet, RakPeerInterface *peer) {}
};