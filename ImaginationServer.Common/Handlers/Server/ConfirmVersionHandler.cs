using System.IO;

namespace ImaginationServer.Common.Handlers.Server
{
    public class ConfirmVersionHandler : PacketHandler
    {
        private readonly LuServer _server;
        private readonly int _port;

        public ConfirmVersionHandler(LuServer server, int port)
        {
            _server = server;
            _port = port;
        }

        public override void Handle(BinaryReader reader, LuClient client)
        {
            //var confirmVersionPacket = new ConfirmVersionResponse();
            //confirmVersionPacket.Serialize(bitStream);
            //LuServer.CurrentServer.Send(bitStream, WPacketPriority.SystemPriority, WPacketReliability.ReliableOrdered, 0, address, false);
            _server.SendInitPacket(_port == 1001, client.Address);
        }
    }
}