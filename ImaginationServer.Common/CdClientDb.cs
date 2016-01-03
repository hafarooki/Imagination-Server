using System;
using System.Data.SQLite;
using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using ImaginationServer.Common.CdClientData;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Properties;
using NHibernate;

namespace ImaginationServer.Common
{
    public class CdClientDb : IDisposable
    {
        public static ISessionFactory Factory { get; set; }

        public static void Init()
        {
            File.WriteAllBytes("cdclient.db", Resources.CDClient);
            Factory =
                Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.UsingFile("cdclient.db"))
                    .Mappings(x => x.FluentMappings.Add<MissionMap>())
                    .BuildSessionFactory();
        }

        private readonly ISession _session;

        public CdClientDb()
        {
            _session = Factory.OpenSession();
        }

        public Mission GetMission(int id)
        {
            return _session.Get<Mission>(id);
        }

        public void Dispose()
        {
            _session.Dispose();
        }
    }
}