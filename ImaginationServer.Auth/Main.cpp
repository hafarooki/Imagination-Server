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
	Server::CreateInstance("Auth");
	
	HandshakeHandler^ handshakeHandler = gcnew HandshakeHandler(SERVER_ADDRESS);
	AuthHandler^ authHandler = gcnew AuthHandler(SERVER_ADDRESS);
	Server::Instance()->AddHandler(handshakeHandler);
	Server::Instance()->AddHandler(authHandler);
	Server::Instance()->Start(SERVER_PORT, MAX_CLIENTS, SERVER_ADDRESS);
	Server::Instance()->Listen();
	delete Server::Instance();
}