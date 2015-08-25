// Credit to LUNI for most of this.

#pragma once

#include <iostream>
#include "MessageIdentifiers.h"
#include <BitStream.h>
#include <iostream>
#include <string.h>
#include <intrin.h>
#include <string>

using namespace RakNet;
using namespace std;

// RemoteConnection IDs
enum RemoteConnection : unsigned char
{
	GENERAL = 0,
	AUTH,
	CHAT,
	UNKNOWNCONN,
	WORLD,
	SERVER
};

// IDs for all servers
enum ServerPacketID : unsigned char
{
	VERSION_CONFIRM = 0,
	DISCONNECT_NOTIFY,
	GENERAL_NOTIFY,
};

// IDs for Auth Server
enum AuthPacketID : unsigned char
{
	LOGIN_REQUEST = 0,
	LOGOUT_REQUEST,
	CREATE_NEW_ACCOUNT_REQUEST,
	LEGOINTERFACE_AUTH_RESPONSE,
	SESSIONKEY_RECIEVED_CONFIRM,
	RUNTIME_CONFIG
};

// IDs for Chat Server
enum ChatPacketID : unsigned char
{
	// IDs for Chat
	LOGIN_SESSION_NOTIFY = 0,
	GENERAL_CHAT_MESSAGE,
	PRIVATE_CHAT_MESSAGE,
	USER_CHANNEL_CHAT_MESSAGE,
	WORLD_DISCONNECT_REQUEST,
	WORLD_PROXIMITY_RESPONSE,
	WORLD_PARCEL_REQUEST,
	ADD_FRIEND_REQUEST,
	ADD_FRIEND_RESPONSE,
	REMOVE_FRIEND,
	GET_FRIENDS_LIST,
	ADD_IGNORE,
	REMOVE_IGNORE,
	GET_IGNORE_LIST,
	TEAM_MISSED_INVITE_CHECK,
	TEAM_INVITE,
	TEAM_INVITE_RESPONSE,
	TEAM_KICK,
	TEAM_LEAVE,
	TEAM_SET_LOOT,
	TEAM_SET_LEADER,
	TEAM_GET_STATUS
};

// IDs for World Server
enum WorldPacketID : unsigned char
{
	CLIENT_VALIDATION = 1,
	CLIENT_CHARACTER_LIST_REQUEST,
	CLIENT_CHARACTER_CREATE_REQUEST,
	CLIENT_LOGIN_REQUEST,
	CLIENT_GAME_MSG,
	CLIENT_CHARACTER_DELETE_REQUEST,
	CLIENT_CHARACTER_RENAME_REQUEST,
	CLIENT_HAPPY_FLOWER_MODE_NOTIFY,
	CLIENT_SLASH_RELOAD_MAP,
	CLIENT_SLASH_PUSH_MAP_REQUEST,
	CLIENT_SLASH_PUSH_MAP,
	CLIENT_SLASH_PULL_MAP,
	CLIENT_LOCK_MAP_REQUEST,
	CLIENT_GENERAL_CHAT_MESSAGE,
	CLIENT_HTTP_MONITOR_INFO_REQUEST,
	CLIENT_SLASH_DEBUG_SCRIPTS,
	CLIENT_MODELS_CLEAR,
	CLIENT_EXIBIT_INSERT_MODEL,
	CLIENT_LEVEL_LOAD_COMPLETE,
	CLIENT_TMP_GUILD_CREATE,
	CLIENT_ROUTE_PACKET,
	CLIENT_POSITION_UPDATE,
	CLIENT_MAIL,
	CLIENT_WORD_CHECK,
	CLIENT_STRING_CHECK
};

struct InitPacket
{
	unsigned long VersionId; // The game version
	unsigned long Unknown1; // Dunno what this is...
	unsigned long RemoteConnectionType; // This the remote connection type (1 = auth, 4 = other)
	unsigned long ProcessId; // The process ID of the server
	unsigned short Unknown2; // Dunno what this is either... it is "FF FF" in hex
	string IpString; // The IP string of the server (will be changed programmatically)

	InitPacket(bool isAuth)
	{
		// Set the variables
		VersionId = 171022;
		Unknown1 = 147;
		RemoteConnectionType = isAuth ? 1 : 4; // Make sure to set this!!!! Determines whether RemoteConnectionType should be 1 or 4
		ProcessId = 5136;
		Unknown2 = 65535;
		IpString = "172.24.8.139";
	}
};

struct LoginStatusPacket
{
	unsigned char loginStatus; // This is the Connection ID
	string talkLikeAPirate; // Always is "Talk_Like_A_Pirate", yet 33 bytes long
	string unknownString; // Have NO idea what this is... Is it relevant?

	unsigned short clientVersion1; // For some reason, the client version
	unsigned short clientVersion2; // is split up over 3 unsigned shorts
	unsigned short clientVersion3; // I believe...

	string unknown; // This is 237 bytes long...

	string userKey; // Generated User Key - Should be wstring
	string redirectIp; // Redirect IP Address
	string chatIp; // Chat IP Address
	unsigned short redirectPort; // Redirect Port
	unsigned short chatPort; // Chat Port
	string anotherIp; // Another IP Address? 33 bytes long
	string possibleGuid; // In the form of a UUID (maybe a GUID?)
	unsigned short zeroShort; // Always 0 for some reason...
	unsigned char localizationChar[3]; // Can be IT, US, etc... Always seems to end in 0
	unsigned char firstLoginSubscription; // Check - Is this the first login after subscribing? 
	unsigned char subscribed; // 0 if subscribed, 1 if not subscribed (for LUNI, always 0)
	unsigned long long zeroLong; // This is all zeros...
	unsigned short errorMsgLength; // This is the length of the error msg
	string ErrorMsg; // This is the error message displayed if connection ID is 0x05. Can be X bytes long (I believe) - Should be wstring

	// Extra bytes
	unsigned short ExtraBytesLength; // This is the number of bytes left (number of each chunk of extra data = 16 bytes * x chunks + these 4 bytes

	// Initializer
	LoginStatusPacket()
	{
	}
};

void CreatePacketHeader(MessageID messageId, unsigned short connectionType, unsigned long internalPacketId, BitStream* output);
