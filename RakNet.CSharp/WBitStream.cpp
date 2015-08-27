#include "Stdafx.h"
#include "WBitStream.h"

RakNetCSharp::WBitStream::WBitStream()
{
	_instance = new BitStream();
}

RakNetCSharp::WBitStream::~WBitStream()
{
	delete _instance;
}

void RakNetCSharp::WBitStream::Write(char chars[])
{
	_instance->Write(chars);
}


