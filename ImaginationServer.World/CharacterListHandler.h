#pragma once
#include <Handler.h>
#include <Packets.h>

class CharacterListHandler : public Handler
{
public:
	virtual unsigned char GetType() override
	{
		return SERVER;
	}

	virtual unsigned char GetCode() override
	{
		return MSG_WORLD_CLIENT_CHARACTER_LIST_REQUEST;
	}

	virtual void Handle(Packet *packet, RakPeerInterface *server) override;
};