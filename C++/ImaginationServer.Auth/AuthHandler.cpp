#include "AuthHandler.h"
#include <LoginStatusPacket.h>
#include <MessageIdentifiers.h>

AuthHandler::AuthHandler(char* address)
{
	Address = address;
}

void AuthHandler::Handle(Packet* packet, RakPeerInterface* peer)
{
	BitStream bitStream;
	CreatePacketHeader(ID_USER_PACKET_ENUM, 5, 0, &bitStream);
	auto loginStatusPacket = LoginStatusPacket(Address, SUCCESS);
	loginStatusPacket.Serialize(&bitStream);
	peer->Send(&bitStream, SYSTEM_PRIORITY, RELIABLE_ORDERED, 0, packet->systemAddress, false);
}
