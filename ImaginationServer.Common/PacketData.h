#pragma once

#include <iostream>
#include <BitStream.h>

using namespace std;
using namespace RakNet;

class PacketData
{
public:
	PacketData()
	{
		
	}

	virtual ~PacketData()
	{
	}

	virtual void Serialize(BitStream* output) {}
};