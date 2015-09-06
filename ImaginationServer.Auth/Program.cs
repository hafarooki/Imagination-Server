using System;
using System.Threading;
using ImaginationServer.Auth.Handlers.Auth;
using ImaginationServer.Common;
using ImaginationServer.Common.Data;
using static System.Console;

namespace ImaginationServer.Auth
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                WriteLine("Starting Imagination Server Auth");
                var server = new LuServer(ServerId.Auth, 1001, 1000, "127.0.0.1");
                server.Handlers.Add(
                    new Tuple<ushort, uint>((byte) PacketEnums.RemoteConnection.Auth,
                        (byte) PacketEnums.ClientAuthPacketId.MsgAuthLoginRequest), new LoginRequestHandler());
                if (!server.Multiplexer.GetDatabase().KeyExists("LastUserId"))
                {
                    server.Multiplexer.GetDatabase().StringSet("LastUserId", 1152921504606846994);
                }
                WriteLine("->OK");
                new Thread(() =>
                {
                    while (!Environment.HasShutdownStarted)
                    {
                        var input = ReadLine();
                        if (string.IsNullOrWhiteSpace(input)) continue;
                        var split = input.Split(new[] {' '}, 2);
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
                                WriteLine("printinfo <character or account> <username>");
                                WriteLine("deletecharacter <username>");
                                break;
                            case "deletecharacter":
                                if (cmdArgs?.Length >= 1)
                                {
                                    var name = cmdArgs[0];
                                    if (!server.CacheClient.Exists($"characters:{name.ToLower()}"))
                                    {
                                        WriteLine("Character does not exist.");
                                        continue;
                                    }

                                    var character = server.CacheClient.Get<Character>($"characters:{name.ToLower()}");
                                    var account = server.CacheClient.Get<Account>($"accounts:{character.Owner.ToLower()}");
                                    account.Characters.Remove(character.Minifig.Name);
                                    server.CacheClient.Remove($"characters:{name.ToLower()}");
                                    server.CacheClient.Remove($"accounts:{account.Username.ToLower()}");
                                    server.CacheClient.Add($"accounts:{account.Username.ToLower()}", account);

                                    WriteLine("Success!");
                                    continue;
                                }

                                WriteLine("Invalid Arguments");
                                break;
                            case "printinfo":
                                if (cmdArgs?.Length >= 2)
                                {
                                    var type = cmdArgs[0];
                                    var username = cmdArgs[1];
                                    if (!server.CacheClient.Exists(
                                        $"{(type == "account" ? "accounts" : "characters")}:{username.ToLower()}"))
                                    {
                                        WriteLine("User does not exist.");
                                        continue;
                                    }
                                    var account =
                                        server.CacheClient.Database.StringGet(
                                            $"{(type == "account" ? "accounts" : "characters")}:{username.ToLower()}");
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
                                    server.CacheClient.Add($"accounts:{username.ToLower()}", account);
                                    WriteLine("Success!");
                                    continue;
                                }

                                WriteLine("Invalid Arguments.");
                                break;
                            case "ban":
                                if (cmdArgs?.Length >= 1)
                                {
                                    var username = cmdArgs[0];
                                    if (!server.CacheClient.Exists($"accounts:{username.ToLower()}"))
                                    {
                                        WriteLine("User does not exist.");
                                        continue;
                                    }
                                    var account =
                                        server.CacheClient.Get<Account>($"accounts:{username.ToLower()}");
                                    account.Banned = true;
                                    server.CacheClient.Remove($"accounts:{account.Username.ToLower()}");
                                    server.CacheClient.Add($"accounts:{account.Username.ToLower()}",
                                        account);
                                    WriteLine("Success!");
                                    continue;
                                }

                                WriteLine("Invalid Arguments.");
                                break;
                            case "unban":
                                if (cmdArgs?.Length >= 1)
                                {
                                    var username = cmdArgs[0];
                                    if (!server.CacheClient.Exists($"accounts:{username.ToLower()}"))
                                    {
                                        WriteLine("User does not exist.");
                                        continue;
                                    }
                                    var account =
                                        server.CacheClient.Get<Account>($"accounts:{username.ToLower()}");
                                    account.Banned = false;
                                    server.CacheClient.Remove($"accounts:{account.Username.ToLower()}");
                                    server.CacheClient.Add($"accounts:{account.Username.ToLower()}",
                                        account);
                                    WriteLine("Success!");
                                    continue;
                                }

                                WriteLine("Invalid Arguments.");
                                break;
                            case "removeaccount":
                                if (cmdArgs?.Length >= 1)
                                {
                                    var username = cmdArgs[0];
                                    if (server.CacheClient.Exists($"accounts:{username.ToLower()}"))
                                    {
                                        var account = server.CacheClient.Get<Account>($"accounts:{username.ToLower()}");
                                        foreach (var name in account.Characters)
                                        {
                                            server.CacheClient.Remove($"characters:{name}");
                                        }
                                        server.CacheClient.Remove($"accounts:{username.ToLower()}");
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
                                    WriteLine(
                                        server.CacheClient.Exists($"accounts:{username.ToLower()}"));
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