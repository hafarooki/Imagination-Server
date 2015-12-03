using System;
using System.IO;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using ImaginationServer.Common.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace ImaginationServer.Common
{
    public class SessionHelper
    {
        private static ISessionFactory _sessionFactory;

        public static void Init()
        {
            _sessionFactory = Fluently.Configure()
                .Database(
                    SQLiteConfiguration.Standard
                        .UsingFile("Database.db")
                )
                .Mappings(
                    m =>
                        m.AutoMappings.Add(
                            AutoMap.AssemblyOf<Account>()
                                .Where(x => x.Namespace?.StartsWith("ImaginationServer.Common.Data") ?? false)))
                .ExposeConfiguration(Config).BuildSessionFactory();
        }

        private static void Config(Configuration configuration)
        {
            if (!File.Exists("Database.db"))
                new SchemaExport(configuration).Create(false, true);
        }

        public static ISession CreateSession()
        {
            return _sessionFactory.OpenSession();
        }
    }
}