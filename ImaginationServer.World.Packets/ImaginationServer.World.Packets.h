// ImaginationServer.World.Packets.h

#pragma once

#include "Stdafx.h"
#include <BitStream.h>
#include <MessageIdentifiers.h>

using namespace RakNet;
using namespace System;
using namespace System::Collections::Generic;
using namespace cli;
using namespace ImaginationServer::Common::Data;

namespace ImaginationServerWorldPackets {

	// From LUNI, mostly

	public enum class CharCreatePantsColor : unsigned long {
		PANTS_BRIGHT_RED = 2508,
		PANTS_BRIGHT_ORANGE = 2509,
		PANTS_BRICK_YELLOW = 2511,
		PANTS_MEDIUM_BLUE = 2513,
		PANTS_SAND_GREEN = 2514,
		PANTS_DARK_GREEN = 2515,
		PANTS_EARTH_GREEN = 2516,
		PANTS_EARTH_BLUE = 2517,
		PANTS_BRIGHT_BLUE = 2519,
		PANTS_SAND_BLUE = 2520,
		PANTS_DARK_STONE_GRAY = 2521,
		PANTS_MEDIUM_STONE_GRAY = 2522,
		PANTS_WHITE = 2523,
		PANTS_BLACK = 2524,
		PANTS_REDDISH_BROWN = 2526,
		PANTS_DARK_RED = 2527
	};

	// Struct that puts names to each base shirt ID (for easy use)
	public enum class CharCreateShirtColor : unsigned long {
		SHIRT_BRIGHT_RED = 4049,
		SHIRT_BRIGHT_BLUE = 4083,
		SHIRT_BRIGHT_YELLOW = 4117,
		SHIRT_DARK_GREEN = 4151,
		SHIRT_BRIGHT_ORANGE = 4185,
		SHIRT_BLACK = 4219,
		SHIRT_DARK_STONE_GRAY = 4253,
		SHIRT_MEDIUM_STONE_GRAY = 4287,
		SHIRT_REDDISH_BROWN = 4321,
		SHIRT_WHITE = 4355,
		SHIRT_MEDIUM_BLUE = 4389,
		SHIRT_DARK_RED = 4423,
		SHIRT_EARTH_BLUE = 4457,
		SHIRT_EARTH_GREEN = 4491,
		SHIRT_BRICK_YELLOW = 4525,
		SHIRT_SAND_BLUE = 4559,
		SHIRT_SAND_GREEN = 4593
	};

	public ref class WorldPackets
	{
	public:
		static void SendCharacterListResponse(String^ address, Account^ account);

		static unsigned long FindCharShirtID(unsigned long shirtColor, unsigned long shirtStyle) {
			unsigned long shirtID = 0;

			// This is a switch case to determine the base shirt color (IDs from CDClient.xml)
			switch (shirtColor) {
			case 0:
			{
				shirtID = shirtStyle >= 35 ? 5730 : (unsigned long)CharCreateShirtColor::SHIRT_BRIGHT_RED;
				break;
			}

			case 1:
			{
				shirtID = shirtStyle >= 35 ? 5736 : (unsigned long)CharCreateShirtColor::SHIRT_BRIGHT_BLUE;
				break;
			}

			case 3:
			{
				shirtID = shirtStyle >= 35 ? 5808 : (unsigned long)CharCreateShirtColor::SHIRT_DARK_GREEN;
				break;
			}

			case 5:
			{
				shirtID = shirtStyle >= 35 ? 5754 : (unsigned long)CharCreateShirtColor::SHIRT_BRIGHT_ORANGE;
				break;
			}

			case 6:
			{
				shirtID = shirtStyle >= 35 ? 5760 : (unsigned long)CharCreateShirtColor::SHIRT_BLACK;
				break;
			}

			case 7:
			{
				shirtID = shirtStyle >= 35 ? 5766 : (unsigned long)CharCreateShirtColor::SHIRT_DARK_STONE_GRAY;
				break;
			}

			case 8:
			{
				shirtID = shirtStyle >= 35 ? 5772 : (unsigned long)CharCreateShirtColor::SHIRT_MEDIUM_STONE_GRAY;
				break;
			}

			case 9:
			{
				shirtID = shirtStyle >= 35 ? 5778 : (unsigned long)CharCreateShirtColor::SHIRT_REDDISH_BROWN;
				break;
			}

			case 10:
			{
				shirtID = shirtStyle >= 35 ? 5784 : (unsigned long)CharCreateShirtColor::SHIRT_WHITE;
				break;
			}

			case 11:
			{
				shirtID = shirtStyle >= 35 ? 5802 : (unsigned long)CharCreateShirtColor::SHIRT_MEDIUM_BLUE;
				break;
			}

			case 13:
			{
				shirtID = shirtStyle >= 35 ? 5796 : (unsigned long)CharCreateShirtColor::SHIRT_DARK_RED;
				break;
			}

			case 14:
			{
				shirtID = shirtStyle >= 35 ? 5802 : (unsigned long)CharCreateShirtColor::SHIRT_EARTH_BLUE;
				break;
			}

			case 15:
			{
				shirtID = shirtStyle >= 35 ? 5808 : (unsigned long)CharCreateShirtColor::SHIRT_EARTH_GREEN;
				break;
			}

			case 16:
			{
				shirtID = shirtStyle >= 35 ? 5814 : (unsigned long)CharCreateShirtColor::SHIRT_BRICK_YELLOW;
				break;
			}

			case 84:
			{
				shirtID = shirtStyle >= 35 ? 5820 : (unsigned long)CharCreateShirtColor::SHIRT_SAND_BLUE;
				break;
			}

			case 96:
			{
				shirtID = shirtStyle >= 35 ? 5826 : (unsigned long)CharCreateShirtColor::SHIRT_SAND_GREEN;
				shirtColor = 16;
				break;
			}
			}

			// Initialize another variable for the shirt color
			unsigned long editedShirtColor = shirtID;

			// This will be the final shirt ID
			unsigned long shirtIDFinal;

			// For some reason, if the shirt color is 35 - 40,
			// The ID is different than the original... Was this because
			// these shirts were added later?
			if (shirtStyle >= 35) {
				shirtIDFinal = editedShirtColor += (shirtStyle - 35);
			}
			else {
				// Get the final ID of the shirt by adding the shirt
				// style to the editedShirtColor
				shirtIDFinal = editedShirtColor += (shirtStyle - 1);
			}

			//cout << "Shirt ID is: " << shirtIDFinal << endl;

			return shirtIDFinal;
		}

		static void WriteStringToBitStream(const char* myString, int stringSize, int maxChars, RakNet::BitStream* output)
		{
			// Write the string to provided BitStream along with the size
			output->Write(myString, stringSize);

			// Check to see if there are any bytes remaining according to user
			// specification
			auto remaining = maxChars - stringSize;

			// If so, fill with 0x00
			for (auto i = 0; i < remaining; i++)
			{
				unsigned char zero = 0;

				output->Write(zero);
			}
		}

		static void CreatePacketHeader(MessageID messageId, unsigned short connectionType, unsigned long internalPacketId, BitStream* output)
		{
			unsigned char unknown1 = 0; // This is an unknown uchar

			// Write data to provided BitStream
			output->Write(messageId);
			output->Write(connectionType);
			output->Write(internalPacketId);
			output->Write(unknown1);
		}
	};
}

