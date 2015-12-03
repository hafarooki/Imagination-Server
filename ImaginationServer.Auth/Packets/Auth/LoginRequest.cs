using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Packets;

namespace ImaginationServer.Auth.Packets.Auth
{
    public class LoginRequest : IncomingPacket
    {
        public string Username { get; }
        public string Password { get; }

        public LoginRequest(BinaryReader reader) : base(reader)
        {
            Username = reader.ReadWString(66); // Read username
            reader.BaseStream.Position = 74; // password starts at 74
            Password = reader.ReadWString(66); // Read password
        }
    }
}