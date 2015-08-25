#include <iostream>

#include <RakPeerInterface.h>
#include <RakNetworkFactory.h>
#include <MessageIdentifiers.h>
#include <map>
#include "Handler.h"
#include <vector>

using namespace std;
using namespace RakNet;

class Server
{
public:
	RakPeerInterface *Peer;
	Handler Handlers[6 * 255];
	char* Name;

	Server(char* name);
	~Server();

	void Start(unsigned short port, int maxConnections, char *address);
	void Listen();
	void AddHandler(Handler handler);
};