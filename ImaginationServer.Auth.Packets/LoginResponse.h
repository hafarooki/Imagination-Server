#pragma once

#include "Stdafx.h"
#include <string>

using namespace std;
using namespace System;

struct LoginResponse
{
public:
	unsigned char LoginStatus; // This is the Connection ID
	string TalkLikeAPirate; // Always is "Talk_Like_A_Pirate", yet 33 bytes long
	string UnknownString; // Have NO idea what this is... Is it relevant?

	unsigned short ClientVersion1; // For some reason, the client version
	unsigned short ClientVersion2; // is split up over 3 unsigned shorts
	unsigned short ClientVersion3; // I believe...

	string Unknown; // This is 237 bytes long...

	string UserKey; // Generated User Key - Should be wstring
	string RedirectIp; // Redirect IP Address
	string ChatIp; // Chat IP Address
	unsigned short RedirectPort; // Redirect Port
	unsigned short ChatPort; // Chat Port
	string AnotherIp; // Another IP Address? 33 bytes long
	string PossibleGuid; // In the form of a UUID (maybe a GUID?)
	unsigned short ZeroShort; // Always 0 for some reason...
	unsigned char LocalizationChar[3]; // Can be IT, US, etc... Always seems to end in 0
	unsigned char FirstLoginSubscription; // Check - Is this the first login after subscribing? 
	unsigned char Subscribed; // 0 if subscribed, 1 if not subscribed (for LUNI, always 0)
	unsigned long long ZeroLong; // This is all zeros...
	unsigned short ErrorMsgLength; // This is the length of the error msg
	string ErrorMsg; // This is the error message displayed if connection ID is 0x05. Can be X bytes long (I believe) - Should be wstring

	unsigned short ExtraBytesLength; // This is the number of bytes left (number of each chunk of extra data = 16 bytes * x chunks + these 4 bytes

	LoginResponse(unsigned char status, String^ userKey);
};

