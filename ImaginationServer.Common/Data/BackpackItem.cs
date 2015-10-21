using StackExchange.Redis;

namespace ImaginationServer.Common.Data
{
    public struct BackpackItem
    {
        public long ObjId { get; set; }
        public int Slot { get; set; }
        public int Count { get; set; }
        public bool Linked { get; set; }

        public BackpackItem(long objId = 0, int slot = 0, int count = 1, bool linked = false)
        {
            ObjId = objId;
            Slot = slot;
            Count = count;
            Linked = linked;
        }
    }
}