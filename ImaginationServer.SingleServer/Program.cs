using System;
using FluentNHibernate.Cfg;
using ImaginationServer.Auth;
using ImaginationServer.Common;
using ImaginationServer.World;

namespace ImaginationServer.SingleServer
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting Imagination Server!");
            try
            {
                Console.WriteLine("Setting up database...");
                SessionHelper.Init();
                Console.WriteLine(" ->OK");
                Console.WriteLine("Setting up CDClient Database...");
                CdClientDb.Init();
                Console.WriteLine(" ->OK");
            }
            catch(FluentConfigurationException exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.InnerException);
                foreach(var reason in exception.PotentialReasons) Console.WriteLine(" - " + reason);
            }
            Console.WriteLine("Starting Auth...");
            AuthServer.Init();
            Console.WriteLine(" ->OK");
            Console.WriteLine("Starting World...");
            WorldServer.Init();
            Console.WriteLine(" ->OK");
            Console.WriteLine("Beginning message receiving...");
            while (!Environment.HasShutdownStarted)
            {
                AuthServer.Service();
                WorldServer.Service();
            }
        }
    }
}
