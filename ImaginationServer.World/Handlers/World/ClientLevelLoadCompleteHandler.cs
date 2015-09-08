using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Handlers;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientLevelLoadCompleteHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, string address)
        {
            var zone = (ZoneId) reader.ReadUInt16();
            var instance = reader.ReadUInt16();
            var clone = reader.ReadInt32();
        }
    }
}