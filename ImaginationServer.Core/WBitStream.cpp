#include "WBitStream.h"

using namespace System::Runtime::InteropServices;
using namespace cli;

WBitStream::WBitStream()
{
	Instance = new BitStream();
	_destroyBitstream = true;
}

WBitStream::WBitStream(unsigned char* bytes, const int length, bool copyData)
{
	Instance = new BitStream(bytes, length, copyData);
	_destroyBitstream = true;
}

WBitStream::WBitStream(BitStream* bitStream)
{
	_destroyBitstream = false;
	Instance = bitStream;
}

WBitStream::~WBitStream()
{
	if (_destroyBitstream) delete Instance;
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
	string chars = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
	cout << chars.c_str() << endl;
	for (unsigned long i = 0; i < chars.size(); i++) {
		Instance->Write(chars.at(i));
	}
}

void WBitStream::Write(WBitStream^ value, unsigned int length) {
	Instance->Write(value->Instance, length);
}

unsigned long WBitStream::GetNumberOfBytesUsed() {
	return Instance->GetNumberOfBytesUsed();
}

void WBitStream::WriteString(String^ value, int maxLength)
{
	WriteString(value, value->Length, maxLength);
}

void WBitStream::Write(Single value)
{
	Instance->Write(value);
}

void WBitStream::WriteString(String^ value, int length, int maxLength)
{
	string chars = (char*)(void*)Marshal::StringToHGlobalAnsi(value);
	cout << chars.c_str() << endl;
	Instance->Write(chars.c_str(), length);
	int remaining = maxLength - length;
	for (int i = 0; i < remaining; i++) Instance->Write((unsigned char)0);
}

void WBitStream::WriteWString(String^ value, bool writeSize, bool nullChar)
{
	wstring str = (wchar_t*)(void*)Marshal::StringToHGlobalUni(value);
	if (nullChar) str.append(L"\0");
	if (writeSize) Instance->Write((unsigned char)(str.length() * 2));
	for (auto i = 0; i < str.size(); i++)
	{
		Instance->Write(str[i]);
	}
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

void WBitStream::Write(bool value)
{
	Instance->Write(value);
}

void WBitStream::Write(cli::array<unsigned char>^ value)
{
	unsigned char* native;
	for (int i = 0; i < value->Length; ++i)
	native[i] = value[i];
	Write(native);
}

void WBitStream::Write(wstring str, bool writeSize, bool nullChar)
{
	if (writeSize) Instance->Write((unsigned char)(str.size() * 2));
	if (nullChar) str.append(L"\0");
	for (unsigned int k = 0; k < str.size(); k++)
	{
		Instance->Write(str.at(k));
}
}

void WBitStream::MarshalString(String ^ s, string& os) {
	using namespace Runtime::InteropServices;
	const char* chars =
	(const char*)(Marshal::StringToHGlobalAnsi(s)).ToPointer();
	os = chars;
	Marshal::FreeHGlobal(IntPtr((void*)chars));
}

//void WBitStream::MarshalString(String ^ s, wstring& os) {
//	using namespace Runtime::InteropServices;
//	const wchar_t* chars =
//		(const wchar_t*)(Marshal::StringToHGlobalUni(s)).ToPointer();
//	os = chars;
//	Marshal::FreeHGlobal(IntPtr((void*)chars));
//}

cli::array<unsigned char>^ WBitStream::GetBytes() {
	auto data = Instance->GetData();
	cli::array<unsigned char>^ data_array = gcnew cli::array<unsigned char>(Instance->GetNumberOfBytesUsed());
	for (int i = 0; i < data_array->Length; ++i)
	data_array[i] = data[i];
	return data_array;
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




