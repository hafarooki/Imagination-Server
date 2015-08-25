#include <RakPeerInterface.h>

#pragma once
class Handler abstract
{
public:
	virtual unsigned char GetType() = 0;
	virtual unsigned char GetCode() = 0;
	Handler();
	~Handler();
	virtual void Handle(Packet *packet, RakPeerInterface *peer) = 0;
};