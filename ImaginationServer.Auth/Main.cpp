#include <iostream>
#include "RakNetworkFactory.h"
#include "RakPeerInterface.h"
#include "MessageIdentifiers.h"
#include <fstream>
#include <sstream>
#include <string>
#include <map>
#include "Handler.h"

using namespace std;
using namespace RakNet;

#define MAX_CLIENTS 100
#define SERVER_PORT 1001

int main(char* args)
{
	RakPeerInterface *peer = RakNetworkFactory::GetRakPeerInterface();
	peer->Startup(MAX_CLIENTS, 30, &SocketDescriptor(SERVER_PORT, 0), 1);
	peer->SetMaximumIncomingConnections(MAX_CLIENTS);

	Packet *packet;

	map<unsigned char, Handler> packetHandlers;

	while (1)
	{
		packet = peer->Receive();

		while (packet)
		{
			packetHandlers[packet->data[0]].Handle(packet);
			peer->DeallocatePacket(packet);
			packet = peer->Receive();
		}
	}

	RakNetworkFactory::DestroyRakPeerInterface(peer);
}