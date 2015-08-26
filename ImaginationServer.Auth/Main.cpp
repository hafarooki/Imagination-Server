#include <Server.h>
#include "HandlerList.h"

using namespace std;
using namespace RakNet;

#define MAX_CLIENTS 100
#define SERVER_PORT 1001
#define SERVER_ADDRESS "127.0.0.1"

int main(char* args)
{
	Server server("Auth");
	
	Handler *handshakeHandler = new HandshakeHandler();
	Handler *authHandler = new AuthHandler();
	server.AddHandler(handshakeHandler);
	server.AddHandler(authHandler);
	server.Start(SERVER_PORT, MAX_CLIENTS, SERVER_ADDRESS);
	server.Listen();
	delete &server;
}