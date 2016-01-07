#pragma once
#include <Replica.h>
#include "BaseServer.h"
#include "WBitStream.h"
#include "ReplicaComponent.h"

using namespace System::Collections::Generic;

class ReplicaMember;

public ref class ReplicaObject
{
	ReplicaMember *_member;
public:
	BaseServer^ Server;
	long long ObjectId;
	String^ Name;
	unsigned long Lot;
	unsigned char GmLevel = 0;
	unsigned short World;

	ReplicaObject(BaseServer^ server);
	~ReplicaObject();

	List<ReplicaComponent^>^ Components;

	void WriteToPacket(WBitStream^ bitStream, ReplicaPacketType type);

	ReplicaMember* GetMember() { return _member; }
};