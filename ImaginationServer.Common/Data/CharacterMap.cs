using FluentNHibernate.Mapping;

namespace ImaginationServer.Common.Data
{
    public class CharacterMap : ClassMap<Character>
    {
        public CharacterMap()
        {
            Id(x => x.Id);

            Map(x => x.Owner);

            Map(x => x.Name);
            Map(x => x.Name1);
            Map(x => x.Name2);
            Map(x => x.Name3);

            Map(x => x.ZoneId);
            Map(x => x.MapInstance);
            Map(x => x.MapClone);

            Map(x => x.Position);
            Map(x => x.Health);
            Map(x => x.MaxHealth);
            Map(x => x.Imagination);
            Map(x => x.MaxImagination);
            Map(x => x.GmLevel);
            Map(x => x.Reputation);
            Map(x => x.Level);
            HasMany(x => x.Items).Cascade.All();
            HasMany(x => x.Missions).Element("Mission").Cascade.AllDeleteOrphan();
            Map(x => x.BackpackSpace);

            Map(x => x.ShirtColor);
            Map(x => x.ShirtStyle);
            Map(x => x.PantsColor);
            Map(x => x.HairStyle);
            Map(x => x.HairColor);
            Map(x => x.Lh);
            Map(x => x.Rh);
            Map(x => x.Eyebrows);
            Map(x => x.Eyes);
            Map(x => x.Mouth);
        }
    }
}