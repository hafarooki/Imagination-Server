using System.Collections.Generic;
using System.IO;
using static ImaginationServer.Common.PacketEnums;

namespace ImaginationServer.Common
{
    public static class ExtensionMethods
    {
        public static string ReadWString(this BinaryReader reader, int maxLength)
        {
            var valueChars = new List<char>();
            while (true)
            {
                var c = reader.ReadChar();
                if (c == '\0' || valueChars.Count >= maxLength) break;
                valueChars.Add(c);
            }
            return new string(valueChars.ToArray());
        }

        public static void WriteHeader(this WBitStream bitStream, RemoteConnection remoteConnection, uint packetCode)
        {
            bitStream.Write((byte)83);
            bitStream.Write((ushort)remoteConnection);
            bitStream.Write(packetCode);
            bitStream.Write((byte)0);
        }
    }
}