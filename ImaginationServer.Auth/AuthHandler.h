#pragma once

#include <Handler.h>
#include <Packets.h>
#include <User.h>

class AuthHandler : public Handler
{
public:
	virtual unsigned char GetType() override
	{ return AUTH; }
	virtual unsigned char GetCode() override
	{ return LOGIN_REQUEST; }

	virtual void Handle(Packet* packet, RakPeerInterface* peer) override;
};