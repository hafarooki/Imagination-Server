#include <Server.h>
#include "HandshakeHandler.h"
#include "AuthHandler.h"

using namespace std;
using namespace RakNet;

#define MAX_CLIENTS 100
#define SERVER_PORT 1001
#define SERVER_ADDRESS "127.0.0.1"

int main(char* args)
{
	Server server("Auth");
	
	HandshakeHandler handshakeHandler(SERVER_ADDRESS);
	AuthHandler authHandler(SERVER_ADDRESS);
	server.AddHandler(&handshakeHandler);
	server.AddHandler(&authHandler);
	server.Start(SERVER_PORT, MAX_CLIENTS, SERVER_ADDRESS);
	server.Listen();
	delete &server;
}