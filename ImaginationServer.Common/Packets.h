#pragma once

#include "MessageIdentifiers.h"
#include <BitStream.h>
#include <iostream>
#include <string.h>
#include <intrin.h>

using namespace RakNet;

// RemoteConnection IDs
enum RemoteConnection : unsigned char {
	GENERAL = 0,
	AUTH,
	CHAT,
	UNKNOWNCONN,
	WORLD,
	SERVER
};

// IDs for all servers
enum ServerPacketID : unsigned char {
	VERSION_CONFIRM = 0,
	DISCONNECT_NOTIFY,
	GENERAL_NOTIFY,
};

// IDs for Auth Server
enum AuthPacketID : unsigned char {
	LOGIN_REQUEST = 0,
	LOGOUT_REQUEST,
	CREATE_NEW_ACCOUNT_REQUEST,
	LEGOINTERFACE_AUTH_RESPONSE,
	SESSIONKEY_RECIEVED_CONFIRM,
	RUNTIME_CONFIG
};

// IDs for Chat Server
enum ChatPacketID : unsigned char {
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
enum WorldPacketID : unsigned char {
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

struct InitPacket {
	unsigned long versionId; // The game version
	unsigned long unknown1; // Dunno what this is...
	unsigned long remoteConnectionType; // This the remote connection type (1 = auth, 4 = other)
	unsigned long processId; // The process ID of the server
	unsigned short unknown2; // Dunno what this is either... it is "FF FF" in hex
	std::string ipString; // The IP string of the server (will be changed programmatically)

	InitPacket() {}

	InitPacket(bool isAuth) {
		// Set the variables
		versionId = 171022;
		unknown1 = 147;
		remoteConnectionType = isAuth ? 1 : 4; // Make sure to set this!!!! Determines whether remoteConnectionType should be 1 or 4
		processId = 5136;
		unknown2 = 65535;
		ipString = "172.24.8.139";
	}
};

void CreatePacketHeader(MessageID messageId, unsigned short connectionType, unsigned long internalPacketId, BitStream *output);