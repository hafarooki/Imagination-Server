#include <iostream>
#include "RakPeerInterface.h"
#include <Server.hpp>
#include "HandshakeHandler.h"
#include "CharacterListHandler.h"

using namespace std;
using namespace RakNet;

#define MAX_CLIENTS 100
#define SERVER_PORT 2003
#define SERVER_ADDRESS "127.0.0.1"

int main(char* args)
{
	ServerInstance("World");

	HandshakeHandler handshakeHandler(SERVER_ADDRESS);
	CharacterListHandler characterListHandler;
	ServerInstance()->AddHandler(&handshakeHandler);
	ServerInstance()->AddHandler(&characterListHandler);
	ServerInstance()->Start(SERVER_PORT, MAX_CLIENTS, SERVER_ADDRESS);
	ServerInstance()->Listen();
	delete ServerInstance();
}