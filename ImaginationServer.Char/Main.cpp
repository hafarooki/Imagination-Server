#include <iostream>
#include "RakPeerInterface.h"
#include <Server.h>

using namespace std;
using namespace RakNet;

#define MAX_CLIENTS 100
#define SERVER_PORT 2006
#define SERVER_ADDRESS "127.0.0.1"

int main(char* args)
{
	Server server("Char");

	server.Start(SERVER_PORT, MAX_CLIENTS, SERVER_ADDRESS);
	server.Listen();
	delete &server;
}