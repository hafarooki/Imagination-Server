#pragma once

#include "Stdafx.h"
#include <NetworkIDManager.h>

public ref class LuNetworkIdManager
{
public:
	NetworkIDManager* Instance;

	LuNetworkIdManager();
	~LuNetworkIdManager();
};