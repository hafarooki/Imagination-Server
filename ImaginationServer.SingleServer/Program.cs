using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using StackExchange.Redis;

namespace ImaginationServer.SingleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.GetCurrentProcess().EnableRaisingEvents = true;

            var multiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");

            Console.Title = "Imagination Server";

            new Thread(() =>
            {
                var auth = -1;
                var character = -1;
                var venture = -1;

                while (!Environment.HasShutdownStarted)
                {
                    try
                    {
                        Process.GetProcessById(auth);
                    }
                    catch
                    {
                        var id = Process.Start(@"..\Auth\ImaginationServer.Auth.exe")?.Id;
                        if (id != null)
                            auth = (int)id;
                    }
                    try
                    {
                        Process.GetProcessById(character);
                    }
                    catch
                    {
                        var id = Process.Start(@"..\World\ImaginationServer.World.exe", "Character")?.Id;
                        if (id != null)
                            character = (int)id;
                    }
                    try
                    {
                        Process.GetProcessById(venture);
                    }
                    catch
                    {
                        var id = Process.Start(@"..\World\ImaginationServer.World.exe", "VentureExplorer")?.Id;
                        if (id != null)
                            venture = (int)id;
                    }

                    Thread.Sleep(250);
                }

                var authProcess = Process.GetProcessById(auth);
                var characterProcess = Process.GetProcessById(character);
                var ventureProcess = Process.GetProcessById(venture);
                authProcess.Kill();
                characterProcess.Kill();
                ventureProcess.Kill();
            }).Start();

            var seconds = 0M;

            Process.GetCurrentProcess().Exited += (a, b) => { multiplexer.GetSubscriber().Publish("Kill", 0); Console.WriteLine("test");};

            while (!Process.GetCurrentProcess().HasExited)
            {
                Console.Clear();
                var secs = seconds % 60;
                var minutes = seconds % (60 * 60);
                minutes -= secs;
                minutes /= 60;
                var hours = seconds;
                hours -= minutes * 60;
                hours -= secs;
                hours /= 60 * 60;

                Console.WriteLine("Hours run: " + hours);
                Console.WriteLine("Minutes run: " + minutes);
                Console.WriteLine("Seconds run: " + secs);
                Thread.Sleep(1000);
                seconds++;
            }
        }
    }
}
