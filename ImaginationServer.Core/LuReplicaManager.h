#pragma once

#include "Stdafx.h"
#include <ReplicaManager.h>
#include <RakNetworkFactory.h>

public ref class LuReplicaManager
{
public:
	ReplicaManager* Instance;

	LuReplicaManager();

	~LuReplicaManager();
};