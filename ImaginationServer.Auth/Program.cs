using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImaginationServer.Common;

namespace ImaginationServer.Auth
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                Console.WriteLine("Starting Imagination Server Auth");
                var server = new LuServer(ServerId.Auth, 1001, 1000, "127.0.0.1");
                Console.WriteLine("->OK");
                server.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
