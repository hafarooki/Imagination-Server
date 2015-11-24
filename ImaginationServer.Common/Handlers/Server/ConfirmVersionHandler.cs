using System.IO;

namespace ImaginationServer.Common.Handlers.Server
{
    public class ConfirmVersionHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, LuClient client)
        {
            //var confirmVersionPacket = new ConfirmVersionResponse();
            //confirmVersionPacket.Serialize(bitStream);
            //LuServer.CurrentServer.Send(bitStream, WPacketPriority.SystemPriority, WPacketReliability.ReliableOrdered, 0, address, false);
            LuServer.CurrentServer.SendInitPacket(LuServer.CurrentServer.ServerId == ServerId.Auth, client.Address);
        }
    }
}