namespace ImaginationServer.Common.Packets
{
    public abstract class Packet
    {
        public abstract void Serialize(WBitStream bitStream);
    }
}