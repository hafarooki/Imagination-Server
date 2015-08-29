#include <Server.hpp>
#include "HandshakeHandler.h"
#include "AuthHandler.h"

using namespace std;
using namespace RakNet;

#define MAX_CLIENTS 100
#define SERVER_PORT 1001
#define SERVER_ADDRESS "127.0.0.1"

int main(char* args)
{
	ServerInstance("Auth");
	
	HandshakeHandler handshakeHandler(SERVER_ADDRESS);
	AuthHandler authHandler(SERVER_ADDRESS);
	ServerInstance()->AddHandler(&handshakeHandler);
	ServerInstance()->AddHandler(&authHandler);
	ServerInstance()->Start(SERVER_PORT, MAX_CLIENTS, SERVER_ADDRESS);
	ServerInstance()->Listen();
	delete ServerInstance();
}  