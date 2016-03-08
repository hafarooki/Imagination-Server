namespace ImaginationServer.Common.Data
{
    public class BackpackItem
    {
        public virtual int Id { get; set; }
        public virtual long Lot { get; set; }
        public virtual int Slot { get; set; }
        public virtual int Count { get; set; }
        public virtual bool Linked { get; set; }
    }
}