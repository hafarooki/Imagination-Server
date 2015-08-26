#pragma once

#include <Handler.h>
#include <Packets.h>

class HandshakeHandler : public Handler
{
public:
	virtual unsigned char GetType() override
	{
		return SERVER;
	}
	virtual unsigned char GetCode() override
	{
		return VERSION_CONFIRM;
	}

	HandshakeHandler(char* address);
	virtual void Handle(Packet *packet, RakPeerInterface *peer) override;
	char* Address;
};