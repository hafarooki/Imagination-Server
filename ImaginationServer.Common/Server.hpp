#pragma once

#include <iostream>

#include <RakPeerInterface.h>
#include "Handler.h"
#include "sqlite/sqlite3.h"
#include <unordered_map>

using namespace std;
using namespace RakNet;

class Server
{
public:
	RakPeerInterface* Peer;
	unordered_map<unsigned short, Handler*> Handlers;
	char* Name;
	sqlite3* SQLite;

	explicit Server(char* name);
	~Server();

	void Start(unsigned short port, int maxConnections, char* address) const;

	void Listen();
	void AddHandler(Handler* handler);
};

static Server *Instance;

static Server *ServerInstance(char* name = nullptr)
{
	if (!Instance)
	{
		Instance = new Server(name);
	}
	return Instance;
}