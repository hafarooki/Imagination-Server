#pragma once

#include <Handler.h>
#include <Packets.h>

public ref class HandshakeHandler : Handler
{
public:
	virtual unsigned char GetType() override
	{ return SERVER; }
	virtual unsigned char GetCode() override
	{ return MSG_SERVER_VERSION_CONFIRM; }

	explicit HandshakeHandler(char* address);
	virtual void Handle(Packet *packet, RakPeerInterface *peer) override;
	char* Address;
};