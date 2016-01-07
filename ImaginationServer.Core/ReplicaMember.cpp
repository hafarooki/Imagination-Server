#include "ReplicaMember.h"
#include "ReplicaObject.h"

ReplicaMember::ReplicaMember(ReplicaObject^ object)
{
	_replicaObject = object;
}

ReplicaReturnResult ReplicaMember::SendConstruction(RakNetTime currentTime, SystemAddress systemAddress, unsigned int &flags, RakNet::BitStream *outBitStream, bool *includeTimestamp)
{
	auto managedBitstream = gcnew WBitStream(outBitStream);
	_replicaObject->WriteToPacket(managedBitstream, ReplicaPacketType::Construction);
	delete(managedBitstream);
	return REPLICA_PROCESSING_DONE;
}
ReplicaReturnResult ReplicaMember::SendDestruction(RakNet::BitStream *outBitStream, SystemAddress systemAddress, bool *includeTimestamp)
{
	return REPLICA_PROCESSING_DONE;
}
ReplicaReturnResult ReplicaMember::ReceiveDestruction(RakNet::BitStream *inBitStream, SystemAddress systemAddress, RakNetTime timestamp)
{
	return REPLICA_PROCESSING_DONE;
}
ReplicaReturnResult ReplicaMember::SendScopeChange(bool inScope, RakNet::BitStream *outBitStream, RakNetTime currentTime, SystemAddress systemAddress, bool *includeTimestamp)
{
	outBitStream->Write(inScope);
	return REPLICA_PROCESSING_DONE;
}
ReplicaReturnResult ReplicaMember::ReceiveScopeChange(RakNet::BitStream *inBitStream, SystemAddress systemAddress, RakNetTime timestamp)
{
	return REPLICA_PROCESSING_DONE;
}
ReplicaReturnResult ReplicaMember::Serialize(bool *sendTimestamp, RakNet::BitStream *outBitStream, RakNetTime lastSendTime, PacketPriority *priority, PacketReliability *reliability, RakNetTime currentTime, SystemAddress systemAddress, unsigned int &flags)
{
	auto managedBitstream = gcnew WBitStream(outBitStream);
	_replicaObject->WriteToPacket(managedBitstream, ReplicaPacketType::Serialization);
	delete(managedBitstream);
	return REPLICA_PROCESSING_DONE;
}
ReplicaReturnResult ReplicaMember::Deserialize(RakNet::BitStream *inBitStream, RakNetTime timestamp, RakNetTime lastDeserializeTime, SystemAddress systemAddress)
{
	return REPLICA_PROCESSING_DONE;
}
