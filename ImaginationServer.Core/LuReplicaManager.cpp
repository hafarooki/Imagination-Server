#include "Stdafx.h"
#include "LuReplicaManager.h"

LuReplicaManager::LuReplicaManager()
{
	Instance = new ReplicaManager();
}

LuReplicaManager::~LuReplicaManager()
{
	delete Instance;
}