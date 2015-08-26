#include "Handler.h"

unsigned char Handler::GetType()
{ return 0; }

unsigned char Handler::GetCode()
{ return 0; }

void Handler::Handle(Packet* packet, RakPeerInterface* peer)
{ }