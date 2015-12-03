namespace ImaginationServer.Common.Data
{
    public class Minifig
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }
        public virtual uint Name1 { get; set; }
        public virtual uint Name2 { get; set; }
        public virtual uint Name3 { get; set; }

        public virtual uint ShirtColor { get; set; }
        public virtual uint ShirtStyle { get; set; }
        public virtual uint PantsColor { get; set; }
        public virtual uint HairStyle { get; set; }
        public virtual uint HairColor { get; set; }
        public virtual uint Lh { get; set; }
        public virtual uint Rh { get; set; }
        public virtual uint Eyebrows { get; set; }
        public virtual uint Eyes { get; set; }
        public virtual uint Mouth { get; set; }
    }
}