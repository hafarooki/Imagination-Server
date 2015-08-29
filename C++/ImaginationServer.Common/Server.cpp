#include <RakNetworkFactory.h>
#include "Packets.h"
#include "Server.hpp"
#include <MessageIdentifiers.h>

Server::Server(char* name)
{
	Name = name;

	Peer = RakNetworkFactory::GetRakPeerInterface();
}

Server::~Server()
{
	RakNetworkFactory::DestroyRakPeerInterface(Peer);
}

void Server::AddHandler(Handler* handler)
{
	Handlers[handler->GetType()][handler->GetCode()] = handler;
}

void Server::Start(unsigned short port, int maxConnections, char *address) const
{
	cout << "Imagination Server" << endl;
	cout << "Starting server " << Name << "." << endl;

	try
	{
		auto socketDescriptor = SocketDescriptor(port, address);
		Peer->Startup(maxConnections, 30, &socketDescriptor, 1);
	}
	catch (const exception& ex)
	{
		cout << "Failed to start server " << Name << "!" << endl;
		cout << ex.what() << endl;;
	}

	cout << "Server " << Name << " has successfully started." << endl;
}

void Server::Listen()
{
	while (1) {

		Packet *packet = Peer->Receive();

		while (packet) {
			cout << "test" << endl;
			try
			{
				switch (packet->data[0]) {
				case ID_REMOTE_DISCONNECTION_NOTIFICATION:
					cout << "A remote client disconnected." << endl;
					break;
				case ID_REMOTE_CONNECTION_LOST:
					cout << "A remote client lost connection." << endl;
					break;
				case ID_REMOTE_NEW_INCOMING_CONNECTION:
					cout << "A remote client connected." << endl;
					break;
				case ID_NEW_INCOMING_CONNECTION:
					cout << "A client connected." << endl;
					break;
				case ID_NO_FREE_INCOMING_CONNECTIONS:
					cout << "The server is full." << endl;
					break;
				case ID_DISCONNECTION_NOTIFICATION:
					cout << "A client disconnected." << endl;
					break;
				case ID_CONNECTION_LOST:
					cout << "A client lost connection." << endl;
					break;
				case ID_USER_PACKET_ENUM:
					cout << "test" << endl;
					Handler* handler = Handlers[packet->data[1]][packet->data[3]];
					if (handler)
					{
						handler->Handle(packet, Peer);
					}
					else
					{
						cout << "Unknown packet of Type " << RemoteConnection(packet->data[1]) << " and Code " << packet->data[3] << "received!" << endl;
					}
					break;
				}
			}
			catch (exception const& e)
			{
				cout << "Error while handling packet - " << e.what() << endl;
			}
			catch (...)
			{
				cout << "Unknown exception occurred." << endl;
			}

			Peer->DeallocatePacket(packet);
			packet = Peer->Receive();
		}
	}
	//default:
	//	cout << "Unkown packet of ID " << packet->data[0] << " received!" << endl;
	//	break;
}