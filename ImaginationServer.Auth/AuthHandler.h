#pragma once

#include <Handler.h>
#include <Packets.h>

class AuthHandler : public Handler
{
public:
	virtual unsigned char GetType() override
	{ return AUTH; }
	virtual unsigned char GetCode() override
	{ return MSG_AUTH_LOGIN_REQUEST; }

	explicit AuthHandler(char* address);
	virtual void Handle(Packet* packet, RakPeerInterface* peer) override;
	char* Address;
};