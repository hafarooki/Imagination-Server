#include "Stdafx.h"
#include "WBitStream.h"

using namespace System::Runtime::InteropServices;

WBitStream::WBitStream()
{
	Instance = new BitStream();
}

WBitStream::~WBitStream()
{
	delete Instance;
}

void WBitStream::Write(unsigned char value)
{
	Instance->Write(value);
}

void WBitStream::Write(unsigned short value)
{
	Instance->Write(value);
}

void WBitStream::Write(unsigned long value)
{
	Instance->Write(value);
}

void WBitStream::Write(unsigned long long value)
{
	Instance->Write(value);
}

void WBitStream::Write(long value)
{
	Instance->Write(value);
}

void WBitStream::Write(long long value)
{
	Instance->Write(value);
}

void WBitStream::WriteString(String^ value, int length, int maxLength)
{
	Instance->Write((char*)(void*)Marshal::StringToHGlobalAnsi(value), length);
	int remaining = maxLength - length;
	for (int i = 0; i < remaining; i++) Instance->Write((unsigned char)0);
}

void WBitStream::WriteWString(String^ value)
{
	wstring str;
	MarshalString(value, str);
	Instance->Write(str);
}

void WBitStream::MarshalString(String ^ s, string& os) {
	using namespace Runtime::InteropServices;
	const char* chars =
		(const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
	os = chars;
	Marshal::FreeHGlobal(IntPtr((void*)chars));
}

void WBitStream::MarshalString(String ^ s, wstring& os) {
	using namespace Runtime::InteropServices;
	const wchar_t* chars =
		(const wchar_t*)(Marshal::StringToHGlobalUni(s)).ToPointer();
	os = chars;
	Marshal::FreeHGlobal(IntPtr((void*)chars));
}