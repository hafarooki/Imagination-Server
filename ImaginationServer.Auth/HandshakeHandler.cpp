#include "HandshakeHandler.h"
#include <iostream>

using namespace std;

void HandshakeHandler::Handle(Packet *packet, RakPeerInterface *peer)
{
	BitStream bitStream;
	CreatePacketHeader(ID_USER_PACKET_ENUM, 0, 0, &bitStream);
	InitPacket charInitPacket = InitPacket(true);
	bitStream.Write((char*)&charInitPacket, sizeof(charInitPacket));
	peer->Send(&bitStream, SYSTEM_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);
}