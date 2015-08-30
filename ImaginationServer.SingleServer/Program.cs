using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace ImaginationServer.SingleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Imagination Server";

            

            var auth = Process.Start(@"..\Auth\ImaginationServer.Auth.exe")?.Id;
            var world = Process.Start(@"..\World\ImaginationServer.World.exe")?.Id;

            new Thread(() =>
            {
                while (!Environment.HasShutdownStarted)
                {
                    try
                    {
                        Process.GetProcessById(auth.Value);
                    }
                    catch
                    {
                        auth = Process.Start(@"..\Auth\ImaginationServer.Auth.exe")?.Id;
                    }
                    try
                    {
                        Process.GetProcessById(world.Value);
                    }
                    catch
                    {
                        world = Process.Start(@"..\World\ImaginationServer.World.exe")?.Id;
                    }
                }
            }).Start();

            var seconds = 0M;

            while (true)
            {
                Console.Clear();
                var secs = seconds % 60;
                var minutes = seconds%(60*60);
                minutes -= secs;
                minutes /= 60;
                var hours = seconds;
                hours -= minutes * 60;
                hours -= secs;
                hours /= 60*60;

                Console.WriteLine("Hours run: " + hours);
                Console.WriteLine("Minutes run: " + minutes);
                Console.WriteLine("Seconds run: " + secs);
                Thread.Sleep(1000);
                seconds++;
            }
        }
    }
}
