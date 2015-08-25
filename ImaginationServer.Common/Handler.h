#include <RakPeerInterface.h>

#pragma once
class Handler
{
public:
	virtual const unsigned char GetType() { return 0; }
	virtual const unsigned char GetCode() { return 0; }
	Handler();
	~Handler();
	virtual void Handle(Packet *packet, RakPeerInterface *peer) { }
};