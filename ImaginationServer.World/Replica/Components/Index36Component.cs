namespace ImaginationServer.World.Replica.Components
{
    public class Index36Component : ReplicaComponent
    {
        public override void WriteToPacket(WBitStream bitStream, ReplicaPacketType type)
        {
            bitStream.Write(true);
            bitStream.Write((ulong)0);
        }

        public override uint GetComponentId()
        {
            return 107;
        }
    }
}