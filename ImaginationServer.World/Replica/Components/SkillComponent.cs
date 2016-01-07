namespace ImaginationServer.World.Replica.Components
{
    public class SkillComponent : ReplicaComponent
    {
        public override void WriteToPacket(WBitStream bitStream, ReplicaPacketType type)
        {
            if(type == ReplicaPacketType.Construction) bitStream.Write(false);
        }

        public override uint GetComponentId()
        {
            return 9;
        }
    }
}