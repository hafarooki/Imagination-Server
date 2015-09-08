#pragma once

#include "Stdafx.h"
#include <BitStream.h>
#include "ReplicaEnums.h"

using namespace RakNet;

public ref class ReplicaComponent abstract
{
public:
	virtual void WriteToPacket(BitStream* packet, ReplicaPacketType packetType) = 0;
	virtual unsigned int GetComponentID() = 0;
};