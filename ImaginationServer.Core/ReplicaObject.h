#pragma once

#include "Stdafx.h"
#include <Replica.h>
#include "WBitStream.h"
#include "WPacketPriority.h"

public ref class ReplicaObject
{
public:
	Replica* Instance;

	ReplicaObject();
	~ReplicaObject();

	ReplicaReturnResult Serialize(bool sendTimestamp, WBitStream^ bitStream, DateTime^ lastSendTime, WPacketPriority priority, WPacketReliability reliability, DateTime^ currentTime, String^ systemAddress, unsigned int flags);
	ReplicaReturnResult Serialize(bool sendTimestamp, RakNet::BitStream *outBitStream, RakNetTime lastSendTime, PacketPriority priority, PacketReliability reliability, RakNetTime currentTime, SystemAddress systemAddress, unsigned int &flags);
};