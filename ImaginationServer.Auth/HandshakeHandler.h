#pragma once

#include <Handler.h>
#include <Packets.h>

class HandshakeHandler : public Handler
{
public:
	unsigned char GetType() { return GENERAL; }
	unsigned char GetCode() { return VERSION_CONFIRM; }

	void Handle(Packet *packet, RakPeerInterface *peer);
};