using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace ImaginationServer.Common.Data
{
    public class BackpackItemMap : ClassMap<BackpackItem>
    {
        public BackpackItemMap()
        {
            Id(x => x.Id);
            Map(x => x.Lot);
            Map(x => x.Count);
            Map(x => x.Slot);
            Map(x => x.Linked);
        }
    }
}