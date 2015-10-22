using StackExchange.Redis;

namespace ImaginationServer.Common.Data
{
    public struct BackpackItem
    {
        public long ObjId { get; set; }
        public int Slot { get; set; }
        public int Count { get; set; }
        public bool Linked { get; set; }
    }
}