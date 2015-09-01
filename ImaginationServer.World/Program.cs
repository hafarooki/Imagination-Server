using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImaginationServer.Common;

namespace ImaginationServer.World
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting Imagination Server World");
                var server = new LuServer(ServerId.World, 2006, 1000, "127.0.0.1");
                Console.WriteLine("->OK");
                server.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.ReadKey(true);
            }
        }
    }
}
