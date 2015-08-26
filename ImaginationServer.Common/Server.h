#include <iostream>

#include <RakPeerInterface.h>
#include "Handler.h"

using namespace std;
using namespace RakNet;

class Server
{
public:
	RakPeerInterface *Peer;
	Handler *Handlers[6 * 255];
	char* Name;

	explicit Server(char* name);
	~Server();

	void Start(unsigned short port, int maxConnections, char *address) const;
	void Listen();
	void AddHandler(Handler *handler);
};