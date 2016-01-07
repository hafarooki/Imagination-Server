#pragma once

#include "WBitStream.h"
#include "ReplicaMember.h"

#include "ReplicaPacketType.h"

public ref class ReplicaComponent abstract {
public:
	virtual void WriteToPacket(WBitStream^ bitStream, ReplicaPacketType type) = 0;
	virtual unsigned int GetComponentId() = 0;
};
