#pragma once

#include <Handler.h>
#include <Packets.h>

class HandshakeHandler : public Handler
{
public:
	virtual const unsigned char GetType() { return GENERAL; }
	virtual const unsigned char GetCode() { return VERSION_CONFIRM; }

	virtual void Handle(Packet *packet, RakPeerInterface *peer);
};