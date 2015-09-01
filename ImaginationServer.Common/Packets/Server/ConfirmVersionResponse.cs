namespace ImaginationServer.Common.Packets.Server
{
    public class ConfirmVersionResponse : Packet
    {
        public override void Serialize(WBitStream bitStream)
        {
            WriteHeader(bitStream, (ushort) PacketEnums.RemoteConnection.Server, (uint) PacketEnums.ServerPacketId.MsgServerVersionConfirm);
            bitStream.Write((uint)171022);
            bitStream.Write((uint)147);
            bitStream.Write((uint)(LuServer.CurrentServer.ServerId == ServerId.Auth ? 1 : 4));
            bitStream.Write((uint)5136);
            bitStream.Write((ushort)65535);
            bitStream.WriteString("127.0.0.1", 33);
        }
    }
}