#include "Packets.h"

void CreatePacketHeader(MessageID messageId, unsigned short connectionType, unsigned long internalPacketId, BitStream *output) 
{
	unsigned char unknown1 = 0; // This is an unknown uchar

						// Write data to provided BitStream
	output->Write(messageId);
	output->Write(connectionType);
	output->Write(internalPacketId);
	output->Write(unknown1);
}