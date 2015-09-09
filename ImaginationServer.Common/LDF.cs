using System;

namespace ImaginationServer.Common
{
    public class Ldf : IDisposable
    {
        private uint _keyNumber;
        public WBitStream BitStream;

        public Ldf()
        {
            BitStream = new WBitStream();
            _keyNumber = 0;
        }

        public void Dispose()
        {
            BitStream.Dispose();
        }

        public void WriteFloat(string key, float data)
        {
            //3
            BitStream.WriteWString(key, true, false);
            BitStream.Write((byte) 3);
            BitStream.Write(data);
            _keyNumber++;
        }

        public void WriteS32(string key, int data)
        {
            //1
            BitStream.WriteWString(key, true, false);
            BitStream.Write((byte) 1);
            BitStream.Write(data);
            _keyNumber++;
        }

        public void WriteS64(string key, long data)
        {
            //8
            BitStream.WriteWString(key, true, false);
            BitStream.Write((byte) 8);
            BitStream.Write(data);
            _keyNumber++;
        }

        public void WriteId(string key, long data)
        {
            //9
            BitStream.WriteWString(key, true, false);
            BitStream.Write((byte) 9);
            BitStream.Write(data);
            _keyNumber++;
        }

        public void WriteBool(string key, bool data)
        {
            //7
            BitStream.WriteWString(key, true, false);
            BitStream.Write((byte) 7);
            if (data) BitStream.Write((byte) 1);
            else BitStream.Write((byte) 0);
            _keyNumber++;
        }

        public void WriteWString(string key, string data)
        {
            //0
            BitStream.WriteWString(key, true, false);
            BitStream.Write((byte) 0);
            BitStream.Write((uint) data.Length);
            BitStream.WriteWString(data, false, false);
            _keyNumber++;
        }

        public void WriteBytes(string key, WBitStream data)
        {
            //13
            BitStream.WriteWString(key, true, false);
            BitStream.Write((byte) 13);
            BitStream.Write(data.GetNumberOfBytesUsed());
            BitStream.Write(data, data.GetNumberOfBytesUsed()*8);
            _keyNumber++;
        }

        public void WriteToPacket(WBitStream packet)
        {
            packet.Write(_keyNumber);
            packet.Write(BitStream, BitStream.GetNumberOfBytesUsed()*8);
        }

        public uint GetSize()
        {
            return BitStream.GetNumberOfBytesUsed() + 4;
        }
    }
}