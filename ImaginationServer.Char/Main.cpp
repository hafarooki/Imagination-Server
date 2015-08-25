#include <iostream>
#include "RakNetworkFactory.h"
#include "RakPeerInterface.h"
#include "MessageIdentifiers.h"
#include <fstream>
#include <sstream>
#include <string>

#include "Handler.h"

using namespace std;
using namespace RakNet;

#define MAX_CLIENTS 100
#define SERVER_PORT 2006

int main(char* args)
{
	Server server();
}