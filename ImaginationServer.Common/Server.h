#include <iostream>

#include <RakPeerInterface.h>
#include <RakNetworkFactory.h>
#include <MessageIdentifiers.h>
#include <map>
#include "Handler.h"

using namespace std;
using namespace RakNet;

class Server
{
public:
	RakPeerInterface *Peer;
	char* Name;
	Handler* Handlers[5 * 255];

	Server(char* name);
	~Server();

	void Start(unsigned short port, int maxConnections, char *address);
	void Listen();
	void AddHandler(Handler &handler);
};