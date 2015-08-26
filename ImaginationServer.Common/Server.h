#include <iostream>

#include <RakPeerInterface.h>
#include "Handler.h"
#include "sqlite/sqlite3.h"

using namespace std;
using namespace RakNet;

class Server
{
public:
	RakPeerInterface* Peer;
	Handler* Handlers[6 * 255];
	char* Name;
	sqlite3* SQLite;

	explicit Server(char* name);
	~Server();

	void Start(unsigned short port, int maxConnections, char* address);

	void Listen();
	void AddHandler(Handler* handler);
};
