#include "Stdafx.h"
#include "LoginResponse.h"
#include <iostream>

using namespace System::Runtime::InteropServices;
using namespace System;
using namespace std;

LoginResponse::LoginResponse(unsigned char success, String^ userKey)
{
	LoginStatus = success;
	TalkLikeAPirate = "Talk_Like_A_Pirate";
	UnknownString = "";
	ClientVersion1 = 1;
	ClientVersion2 = 10;
	ClientVersion3 = 64;
	Unknown = "_";
	UserKey = (char*)(void*)Marshal::StringToHGlobalAnsi(userKey);
	//UserKey = "0 9 4 e 7 0 1 a c 3 b 5 5 2 0 b 4 7 8 9 5 b 3 1 8 5 7 b f 1 c 3   ";
	ChatIp = "localhost";
	ChatPort = 2003;
	AnotherIp = "localhost";
	PossibleGuid = "00000000-0000-0000-0000-000000000000";
	ZeroShort = 0;
	LocalizationChar[0] = 0x55;
	LocalizationChar[1] = 0x53;
	LocalizationChar[2] = 0x00;
	FirstLoginSubscription = 0;
	Subscribed = 0;
	ZeroLong = 0;
	RedirectIp = "localhost";
	RedirectPort = 2006;
	ErrorMsg = "T";
	ErrorMsgLength = 0;
	ExtraBytesLength = 324;
}
