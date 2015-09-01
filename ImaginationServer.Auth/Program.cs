using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImaginationServer.Auth.Handlers.Auth;
using ImaginationServer.Common;
using ImaginationServer.Common.Data;

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
                server.Handlers.Add(new Tuple<ushort, uint>((byte)PacketEnums.RemoteConnection.Auth, (byte)PacketEnums.ClientAuthPacketId.MsgAuthLoginRequest), new LoginRequestHandler());
                Console.WriteLine("->OK");
                new Thread(() =>
                {
                    while (!Environment.HasShutdownStarted)
                    {
                        var input = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(input)) continue;
                        var split = input.Split(new[] { ' ' }, 2);
                        var cmd = split[0].ToLower();
                        var cmdArgs = split.Length > 1 ? split[1].Split(' ') : null;
                        switch (cmd)
                        {
                            case "help":
                                Console.WriteLine("addaccount <username> <password> <email>");
                                Console.WriteLine("removeaccount <username>");
                                Console.WriteLine("accountexists <username>");
                                break;
                            case "addaccount":
                                if (cmdArgs != null && cmdArgs.Length >= 3)
                                {
                                    var username = cmdArgs[0];
                                    var password = cmdArgs[1];
                                    var email = cmdArgs[2];

                                    var account = Account.Create(username, password, email);
                                    LuServer.CurrentServer.CacheClient.Add("accounts:" + username.ToLower(), account);         
                                    Console.WriteLine("Success!");
                                    continue;
                                }

                                Console.WriteLine("Invalid Arguments.");
                                break;
                            case "removeaccount":
                                if (cmdArgs != null && cmdArgs.Length >= 1)
                                {
                                    var username = cmdArgs[0];
                                    if (LuServer.CurrentServer.CacheClient.Exists("accounts" + username.ToLower()))
                                    {
                                        LuServer.CurrentServer.CacheClient.Remove("accounts:" + username.ToLower());
                                        Console.WriteLine("Success!");
                                        continue;
                                    }

                                    Console.WriteLine("User does not exist.");
                                    continue;
                                }

                                Console.WriteLine("Invalid Arguments.");
                                break;
                            case "accountexists":
                                if (cmdArgs != null && cmdArgs.Length >= 1)
                                {
                                    var username = cmdArgs[0];
                                    Console.WriteLine(LuServer.CurrentServer.CacheClient.Exists("accounts:" + username.ToLower()));
                                    continue;
                                }

                                Console.WriteLine("Invalid Arguments.");
                                break;
                            default:
                                Console.WriteLine("Unknown command.");
                                break;
                        }
                    }
                }).Start();
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
