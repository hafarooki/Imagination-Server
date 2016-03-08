using System;

namespace ImaginationServer.World.Replica.Components
{
    public class DestructibleComponent : ReplicaComponent
    {
        public uint Health { get; set; }
        public uint Armor { get; set; }
        public uint Imagination { get; set; }

        public uint MaxHealth { get; set; }
        public uint MaxArmor { get; set; }
        public uint MaxImagination { get; set; }
        public int? Faction { get; set; }

        public override void WriteToPacket(WBitStream bitStream, ReplicaPacketType type)
        {
            // Index 1
            if (type == ReplicaPacketType.Construction)
            {
                bitStream.Write(false);
                bitStream.Write(false);
            }

            // Index 2
            if (type == ReplicaPacketType.Construction)
            {
                bitStream.Write(false);
            }
            bitStream.Write(Faction == null ? 0 : 1);
            if(Faction != null) bitStream.Write(Faction.Value);
            bitStream.Write(false);
            if (type == ReplicaPacketType.Construction)
            {
                bitStream.Write(false);
                bitStream.Write(false);
                bitStream.Write(false);
            }
            bitStream.Write(false);
        }

        public override uint GetComponentId()
        {
            return 7;
        }
    } 
}