using ImaginationServer.Common.Packets;

namespace ImaginationServer.Auth.Packets.Auth
{
    public class LoginRequest : IncomingPacket
    {
        public string Username { get; }
        public string Password { get; }

        public LoginRequest(byte[] data) : base(data)
        {
            using (var bitStream = new WBitStream(data, false))
            {
                ClearHeader(bitStream);
                Username = bitStream.ReadWString();
                Password = bitStream.ReadWString();
            }
        }
    }
}