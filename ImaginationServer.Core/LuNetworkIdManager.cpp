#include "Stdafx.h"
#include "LuNetworkIdManager.h"

LuNetworkIdManager::LuNetworkIdManager()
{
	Instance = new NetworkIDManager();
}

LuNetworkIdManager::~LuNetworkIdManager()
{
	delete Instance;
}
