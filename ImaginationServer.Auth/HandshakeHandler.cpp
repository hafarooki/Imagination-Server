#include "HandshakeHandler.h"
#include <iostream>
#include <InitPacket.h>

using namespace std;

void HandshakeHandler::Handle(Packet *packet, RakPeerInterface *peer)
{
	BitStream bitStream;
	CreatePacketHeader(ID_USER_PACKET_ENUM, GENERAL, LOGIN_REQUEST, &bitStream);
	auto charInitPacket = InitPacket(true);
	charInitPacket.Serialize(&bitStream);
	peer->Send(&bitStream, SYSTEM_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);
	cout << "test1" << endl;
}