#include "Stdafx.h"
#include "WBitStream.h"

using namespace System::Runtime::InteropServices;

WBitStream::WBitStream()
{
	Instance = new BitStream();
}

WBitStream::WBitStream(unsigned char* bytes, const int length, bool copyData)
{
	Instance = new BitStream(bytes, length, copyData);
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

void WBitStream::WriteChars(String^ value)
{
	Instance->Write((char*)(void*)Marshal::StringToHGlobalAnsi(value));
}

void WBitStream::WriteString(String^ value, int maxLength)
{
	WriteString(value, value->Length, maxLength);
}

void WBitStream::WriteString(String^ value, int length, int maxLength)
{
	auto chars = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
	Instance->Write(chars, length);
	int remaining = maxLength - length;
	for (int i = 0; i < remaining; i++) Instance->Write((unsigned char)0);
}

void WBitStream::WriteWString(String^ value)
{
	wstring str;
	MarshalString(value, str);
	Instance->Write(str);
}

void WBitStream::Write(char * value, int length)
{
	Instance->Write(value, length);
}

void WBitStream::Write(unsigned char * value, int length)
{
	Instance->Write(value), length;
}

void WBitStream::Write(const char * value, int length, int maxLength)
{
	Instance->Write(value, length);
	int remaining = maxLength - length;
	for (int i = 0; i < remaining; i++) Instance->Write((unsigned char)0);
}

void WBitStream::Write(char * value)
{
	Instance->Write(value);
}

void WBitStream::Write(unsigned char * value)
{
	Instance->Write(value);
}

void WBitStream::Write(wstring value)
{
	Instance->Write(value);
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

//unsigned char WBitStream::ReadByte()
//{
//	unsigned char value;
//	Instance->Read(value);
//	return value;
//}
//
//unsigned short WBitStream::ReadUShort()
//{
//	unsigned short value;
//	Instance->Read(value);
//	return value;
//}
//
//unsigned long WBitStream::ReadULong()
//{
//	unsigned long value;
//	Instance->Read(value);
//	return value;
//}
//
//unsigned long long WBitStream::ReadULongLong()
//{
//	unsigned long long value;
//	Instance->Read(value);
//	return value;
//}
//
//long WBitStream::ReadLong()
//{
//	long value;
//	Instance->Read(value);
//	return value;
//}
//
//long long WBitStream::ReadLongLong()
//{
//	long long value;
//	Instance->Read(value);
//	return value;
//}
//
//String^ WBitStream::ReadWString()
//{
//	wstring value;
//	Instance->Read(value);
//	return gcnew String(value.c_str());
//}