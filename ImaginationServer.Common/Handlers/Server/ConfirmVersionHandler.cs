using ImaginationServer.Common.Packets.Server;

namespace ImaginationServer.Common.Handlers.Server
{
    public class ConfirmVersionHandler : PacketHandler
    {
        public override void Handle(byte[] data, string address)
        {
            using (var bitStream = new WBitStream())
            {
                var confirmVersionPacket = new ConfirmVersionPacket();
                confirmVersionPacket.Serialize(bitStream);
                LuServer.CurrentServer.Send(bitStream, WPacketPriority.SystemPriority, WPacketReliability.ReliableOrdered, 0, address, false);
            }
        }
    }
}