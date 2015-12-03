using System.IO;

namespace ImaginationServer.Common.Handlers
{
    public abstract class PacketHandler
    {
        public abstract void Handle(BinaryReader reader, LuClient sender);
    }
}
