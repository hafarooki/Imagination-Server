using System.Collections.Generic;

namespace ImaginationServer.Common.Data
{
    public class Character
    {
        public virtual long Id { get; set; }

        public virtual string Owner { get; set; }
        public virtual Minifig Minifig { get; set; }
        public virtual float[] Position { get; set; }
        public virtual ushort ZoneId { get; set; }
        public virtual ushort MapInstance { get; set; }
        public virtual uint MapClone { get; set; }
        public virtual uint Health { get; set; }
        public virtual float MaxHealth { get; set; }
        public virtual uint Armor { get; set; }
        public virtual float MaxArmor { get; set; }
        public virtual uint Imagination { get; set; }
        public virtual float MaxImagination { get; set; }
        public virtual int GmLevel { get; set; }
        public virtual long Reputation { get; set; }

        public virtual IList<BackpackItem> Items { get; set; }
        public virtual uint Level { get; set; }
        public virtual IList<Mission> Missions { get; set; }

        public virtual void AddItem(BackpackItem backpackItem)
        {
            Items.Add(backpackItem);
        }

        public virtual long GetObjectId()
        {
            return Id + 1152921504606846994;
        }
    }
}