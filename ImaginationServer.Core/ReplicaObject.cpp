#include "ReplicaObject.h"
#include "ReplicaMember.h"
#include "ReplicaComponent.h"

ReplicaObject::ReplicaObject(BaseServer^ server)
{
	Server = server;
	_member = new ReplicaMember(this);
	Components = gcnew List<ReplicaComponent^>();
}

ReplicaObject::~ReplicaObject()
{
	Server->GetReplicaManager()->DereferencePointer(_member);
	delete(_member);
}

void ReplicaObject::WriteToPacket(WBitStream^ bitStream, ReplicaPacketType type)
{
	if(type == ReplicaPacketType::Construction)
	{
		bitStream->Write(ObjectId);
		bitStream->Write(Lot);
		bitStream->WriteWString(Name, true, false);
		bitStream->Write((unsigned long)0);
		bitStream->Write(false);
		bitStream->Write(false);
		bitStream->Write(false);
		bitStream->Write(false);
		bitStream->Write(false);
		bitStream->Write(false);
		bitStream->Write(GmLevel > 0);
		if(GmLevel > 0)
		{
			bitStream->Write(GmLevel);
		}
	}
	bitStream->Write(true);
	bitStream->Write(false);
	bitStream->Write(false);

	for each(auto component in Components)
	{
		component->WriteToPacket(bitStream, type);
	}
}