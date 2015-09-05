using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ImaginationServer.Auth.Handlers.Auth;
using ImaginationServer.Common;
using ImaginationServer.Common.Data;
using Newtonsoft.Json;
using static System.Console;

namespace ImaginationServer.Auth
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                WriteLine("Starting Imagination Server Auth");
                var server = new LuServer(ServerId.Auth, 1001, 1000, "127.0.0.1");
                server.Handlers.Add(new Tuple<ushort, uint>((byte)PacketEnums.RemoteConnection.Auth, (byte)PacketEnums.ClientAuthPacketId.MsgAuthLoginRequest), new LoginRequestHandler());
                WriteLine("->OK");
                new Thread(() =>
                {
                    while (!Environment.HasShutdownStarted)
                    {
                        var input = ReadLine();
                        if (string.IsNullOrWhiteSpace(input)) continue;
                        var split = input.Split(new[] { ' ' }, 2);
                        var cmd = split[0].ToLower();
                        var cmdArgs = split.Length > 1 ? split[1].Split(' ') : null;
                        switch (cmd)
                        {
                            case "help":
                                WriteLine("addaccount <username> <password> <email>");
                                WriteLine("removeaccount <username>");
                                WriteLine("accountexists <username>");
                                WriteLine("ban <username>");
                                WriteLine("unban <username>");
                                WriteLine("printinfo <username>");
                                break;
                            case "printinfo":
                                if (cmdArgs?.Length >= 1)
                                {
                                    var username = cmdArgs[0];
                                    if (!LuServer.CurrentServer.CacheClient.Exists("accounts:" + username.ToLower()))
                                    {
                                        WriteLine("User does not exist.");
                                        continue;
                                    }
                                    var account =
                                        LuServer.CurrentServer.CacheClient.Database.StringGet("accounts:" + username.ToLower());
                                    WriteLine(account);
                                    continue;
                                }

                                WriteLine("Invalid Arguments!");
                                break;
                            case "addaccount":
                                if (cmdArgs != null && cmdArgs.Length >= 3)
                                {
                                    var username = cmdArgs[0];
                                    var password = cmdArgs[1];
                                    var email = cmdArgs[2];

                                    var account = Account.Create(username, password, email);
                                    LuServer.CurrentServer.CacheClient.Add("accounts:" + username.ToLower(), account);         
                                    WriteLine("Success!");
                                    continue;
                                }

                                WriteLine("Invalid Arguments.");
                                break;
                            case "ban":
                                if (cmdArgs?.Length >= 1)
                                {
                                    var username = cmdArgs[0];
                                    if (!LuServer.CurrentServer.CacheClient.Exists("accounts:" + username.ToLower()))
                                    {
                                        WriteLine("User does not exist.");
                                        continue;
                                    }
                                    var account = LuServer.CurrentServer.CacheClient.Get<Account>("accounts:" + username.ToLower());
                                    account.Banned = true;
                                    LuServer.CurrentServer.CacheClient.Remove("accounts:" + account.Username.ToLower());
                                    LuServer.CurrentServer.CacheClient.Add("accounts:" + account.Username.ToLower(), account);
                                    WriteLine("Success!");
                                    continue;
                                }

                                WriteLine("Invalid Arguments.");
                                break;
                            case "unban":
                                if (cmdArgs?.Length >= 1)
                                {
                                    var username = cmdArgs[0];
                                    if (!LuServer.CurrentServer.CacheClient.Exists("accounts:" + username.ToLower()))
                                    {
                                        WriteLine("User does not exist.");
                                        continue;
                                    }
                                    var account = LuServer.CurrentServer.CacheClient.Get<Account>("accounts:" + username.ToLower());
                                    account.Banned = false;
                                    LuServer.CurrentServer.CacheClient.Remove("accounts:" + account.Username.ToLower());
                                    LuServer.CurrentServer.CacheClient.Add("accounts:" + account.Username.ToLower(), account);
                                    WriteLine("Success!");
                                    continue;
                                }

                                WriteLine("Invalid Arguments.");
                                break;
                            case "removeaccount":
                                if (cmdArgs?.Length >= 1)
                                {
                                    var username = cmdArgs[0];
                                    if (LuServer.CurrentServer.CacheClient.Exists("accounts:" + username.ToLower()))
                                    {
                                        LuServer.CurrentServer.CacheClient.Remove("accounts:" + username.ToLower());
                                        WriteLine("Success!");
                                        continue;
                                    }

                                    WriteLine("User does not exist.");
                                    continue;
                                }

                                WriteLine("Invalid Arguments.");
                                break;
                            case "accountexists":
                                if (cmdArgs != null && cmdArgs.Length >= 1)
                                {
                                    var username = cmdArgs[0];
                                    WriteLine(LuServer.CurrentServer.CacheClient.Exists("accounts:" + username.ToLower()));
                                    continue;
                                }

                                WriteLine("Invalid Arguments.");
                                break;
                            default:
                                WriteLine("Unknown command.");
                                break;
                        }
                    }
                }).Start();
                server.Start();
            }
            catch (Exception e)
            {
                WriteLine(e.StackTrace);
                ReadKey(true);
            }
        }
    }
}
