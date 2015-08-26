#include "HandshakeHandler.h"
#include <iostream>
#include <InitPacket.h>

using namespace std;

void HandshakeHandler::Handle(Packet *packet, RakPeerInterface *peer)
{
	BitStream bitStream;
	CreatePacketHeader(ID_USER_PACKET_ENUM, 0, 0, &bitStream);
	auto charInitPacket = InitPacket(true);
	charInitPacket.Serialize(bitStream);
	peer->Send(&bitStream, SYSTEM_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);
	cout << "test1";
}