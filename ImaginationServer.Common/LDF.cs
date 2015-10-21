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

        /// <summary>
        /// 3
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void WriteFloat(string key, float data)
        {
            BitStream.WriteWString(key, true, false);
            BitStream.Write((byte) 3);
            BitStream.Write(data);
            _keyNumber++;
        }

        /// <summary>
        /// 1
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void WriteS32(string key, int data)
        {
            BitStream.WriteWString(key, true, false);
            BitStream.Write((byte) 1);
            BitStream.Write(data);
            _keyNumber++;
        }

        /// <summary>
        /// 8
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void WriteS64(string key, long data)
        {
            BitStream.WriteWString(key, true, false);
            BitStream.Write((byte) 8);
            BitStream.Write(data);
            _keyNumber++;
        }

        /// <summary>
        /// 9
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void WriteId(string key, long data)
        {
            BitStream.WriteWString(key, true, false);
            BitStream.Write((byte) 9);
            BitStream.Write(data);
            _keyNumber++;
        }

        /// <summary>
        /// 7
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void WriteBool(string key, bool data)
        {
            BitStream.WriteWString(key, true, false);
            BitStream.Write((byte) 7);
            if (data) BitStream.Write((byte) 1);
            else BitStream.Write((byte) 0);
            _keyNumber++;
        }

        /// <summary>
        /// 0
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void WriteWString(string key, string data)
        {
            BitStream.WriteWString(key, true, false);
            BitStream.Write((byte) 0);
            BitStream.Write((uint) data.Length);
            BitStream.WriteWString(data, false, false);
            _keyNumber++;
        }

        /// <summary>
        /// 13
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void WriteBytes(string key, WBitStream data)
        {
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