﻿namespace ImaginationServer.Common.Data
{
    public class Character
    {
        public ulong Id { get; set; }

        public string Owner { get; set; }
        public Minifig Minifig { get; set; }
        public float[] Position { get; set; }
        public float[] Rotation { get; set; }
        public ushort ZoneId { get; set; }
        public ushort MapInstance { get; set; }
        public uint MapClone { get; set; }
        public uint Health { get; set; }
        public float MaxHealth { get; set; }
        public uint Armor { get; set; }
        public float MaxArmor { get; set; }
        public uint Imagination { get; set; }
        public float MaxImagination { get; set; }
    }
}