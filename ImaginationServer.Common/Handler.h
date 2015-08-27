#pragma once

#include <RakPeerInterface.h>

class Handler
{
public:
	virtual ~Handler()
	{
	}

	virtual unsigned char GetType();
	virtual unsigned char GetCode();

	virtual void Handle(Packet* packet, RakPeerInterface *peer);
};