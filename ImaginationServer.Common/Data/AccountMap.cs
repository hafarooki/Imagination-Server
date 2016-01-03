using FluentNHibernate.Mapping;

namespace ImaginationServer.Common.Data
{
    public class AccountMap : ClassMap<Account>
    {
        public AccountMap()
        {
            Id(x => x.Id);
            Map(x => x.Created);
            Map(x => x.Banned);

            Map(x => x.Username);
            Map(x => x.Password);
            Map(x => x.Salt);

            HasMany(x => x.Characters).Element("Character").Cascade.AllDeleteOrphan();
            Map(x => x.SelectedCharacter);
        }
    }
}