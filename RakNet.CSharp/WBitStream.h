#pragma once

#include "Stdafx.h"
#include <BitStream.h>

using namespace RakNet;
using namespace RakNet;
using namespace System;

namespace RakNetCSharp
{
	public ref class WBitStream
	{
	private:
		BitStream *_instance;
	public:
		WBitStream();
		~WBitStream();

		void Write(char chars[]);
	};
}