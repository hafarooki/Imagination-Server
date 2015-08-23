#include <RakPeerInterface.h>

#pragma once
class Handler
{
public:
	Handler();
	~Handler();
	void Handle(Packet *packet);
};