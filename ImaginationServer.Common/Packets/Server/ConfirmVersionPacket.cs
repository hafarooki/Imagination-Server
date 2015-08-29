namespace ImaginationServer.Common.Packets.Server
{
    public class ConfirmVersionPacket : Packet
    {
        public override void Serialize(WBitStream bitStream)
        {
            bitStream.Write((ulong)171022);
            bitStream.Write((ulong)0x93);
            bitStream.Write((ulong)(LuServer.CurrentServer.ServerId == ServerId.Auth ? 1 : 4));
            bitStream.Write((ulong)5136);
            bitStream.Write((ushort)0xff);
            bitStream.WriteString("127.0.0.1", "127.0.0.1".Length, "127.0.0.1".Length);
        }
    }
}