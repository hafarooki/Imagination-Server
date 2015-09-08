using System.IO;
using ImaginationServer.Common.Codes;

namespace ImaginationServer.Common.Subscribers
{
    public abstract class PacketSubscriber
    {
        public abstract OpCode Code { get; }
        
        public void MessageReceived(byte[] data)
        {
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                var target = (ServerId)reader.ReadByte();
                if (!LuServer.CurrentServer.ServerId.HasFlag(target)) return;
                var sender = (ServerId) reader.ReadByte();
                Handle(sender, reader);
            }
        }

        protected abstract void Handle(ServerId sender, BinaryReader reader);
    }
}