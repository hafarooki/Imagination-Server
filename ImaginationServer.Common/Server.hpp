#pragma once

#include <iostream>

#include <RakPeerInterface.h>
#include "Handler.h"
#include <unordered_map>

using namespace std;
using namespace RakNet;
using namespace System::Collections::Generic;
using namespace System;

public ref class Server
{
	static Server^ _instance;
public:	
	static void CreateInstance(char* name)
	{
		_instance = gcnew Server(name);
	}

	static Server^ Instance()
	{
		return _instance;
	}

	RakPeerInterface* Peer;
	Dictionary<Tuple<char, char>^, Handler^>^ Handlers;
	char* Name;

	explicit Server(char* name);

	void Start(unsigned short port, int maxConnections, char* address);
	void Stop();

	void Listen();
	void AddHandler(Handler^ handler);
};