namespace ImaginationServer.World.Replica.Components
{
    public class RenderComponent : ReplicaComponent
    {
        public override void WriteToPacket(WBitStream bitStream, ReplicaPacketType type)
        {
            if(type == ReplicaPacketType.Construction) bitStream.Write((uint)0);
        }

        public override uint GetComponentId()
        {
            return 2;
        }
    }
}