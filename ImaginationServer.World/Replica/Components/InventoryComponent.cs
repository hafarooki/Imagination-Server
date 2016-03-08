using System.Collections.Generic;
using ImaginationServer.Common.CharacterData;
using ImaginationServer.Common.Data;

namespace ImaginationServer.World.Replica.Components
{
    public class InventoryComponent : ReplicaComponent
    {
        public override void WriteToPacket(WBitStream bitStream, ReplicaPacketType type)
        {
            bitStream.Write(Flag);
            if (Flag)
            {
                bitStream.Write(Items.Count);
                foreach (var item in Items)
                {
                    bitStream.Write(item.Id);
                    bitStream.Write(item.Lot);
                    bitStream.Write(false);
                    bitStream.Write(true);
                    bitStream.Write((uint)1);
                    bitStream.Write(true);
                    bitStream.Write(item.Slot);
                    bitStream.Write(true);
                    bitStream.Write((uint)4);
                    bitStream.Write(false);
                    bitStream.Write(false);
                }
            }

            bitStream.Write(false);
        }

        public override uint GetComponentId()
        {
            return 17;
        }

        public bool Flag { get; set; }
        public List<BackpackItem> Items { get; set; }
    }
}