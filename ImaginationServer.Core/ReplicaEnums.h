#pragma once

#include "Stdafx.h"

public enum class LOT : long {
	None = 0,
	Player = 1,
	ShirtLuPurple = 16065,
	ShirtLuLightGrey = 16066,
	ShirtLuGrey = 16067,
	ShirtLuGreen = 16068,
	ShirtLuOrange = 16069,
	ShirtLuBeige = 16070,
	ShirtArmorRed = 16071,
	ShirtArmorBlack = 16072,
	ShirtArmorBlue = 16073,
	ShirtArmorWhite = 16074,
	ShirtArmorPurple = 16075,
	ShirtArmorLightGrey = 16077,
	ShirtArmorGrey = 16078,
	ShirtArmorGreenPale = 16079,
	ShirtArmorOrange = 16080,
	ShirtArmorBeige = 16081,
	ShirtOldGrey = 16082,
	Jetpack = 1727,
	KingsCrown = 8544,
	Unknown4 = 8642,
	ShieldWolf = 12677,
	FantasticPilum = 12730
};

public enum class ReplicaPacketType {
	ReplicaConstructionPacket = 0,
	ReplicaSerializationPacket = 1,
	ReplicaDestructionPacket = 2
};