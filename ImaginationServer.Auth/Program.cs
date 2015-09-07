using System;
using System.Threading;
using System.Windows.Forms;
using ImaginationServer.Auth.Handlers.Auth;
using ImaginationServer.Common;
using ImaginationServer.Common.Data;
using static System.Console;
using static ImaginationServer.Common.PacketEnums;
using static ImaginationServer.Common.PacketEnums.ClientAuthPacketId;

namespace ImaginationServer.Auth
{
    internal class Program
    {
        public static LuServer Server;

        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                WriteLine("Starting Imagination Server Auth");
                Server = new LuServer(ServerId.Auth, 1001, 1000, "127.0.0.1");
                Server.AddHandler((byte) RemoteConnection.Auth, (byte) MsgAuthLoginRequest, new LoginRequestHandler());
                if (!Server.Multiplexer.GetDatabase().KeyExists("LastUserId"))
                {
                    Server.Multiplexer.GetDatabase().StringSet("LastUserId", 1152921504606846994);
                }
                WriteLine("->OK");
                new Thread(() => Server.Start()).Start();
                Command();
            }
            catch (Exception e)
            {
                WriteLine(e.StackTrace);
                ReadKey(true);
            }
        }

        [STAThread]
        public static void Command()
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
                        WriteLine("printinfo <character or account> <username> [clipboard]");
                        WriteLine("deletecharacter <username>");
                        break;
                    case "deletecharacter":
                        if (cmdArgs?.Length >= 1)
                        {
                            var name = cmdArgs[0];
                            if (!Server.CacheClient.Exists($"characters:{name.ToLower()}"))
                            {
                                WriteLine("Character does not exist.");
                                continue;
                            }

                            var character = Server.CacheClient.Get<Character>($"characters:{name.ToLower()}");
                            var account = Server.CacheClient.Get<Account>($"accounts:{character.Owner.ToLower()}");
                            account.Characters.Remove(character.Minifig.Name);
                            Server.CacheClient.Remove($"characters:{name.ToLower()}");
                            Server.CacheClient.Remove($"accounts:{account.Username.ToLower()}");
                            Server.CacheClient.Add($"accounts:{account.Username.ToLower()}", account);

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
                            if (!Server.CacheClient.Exists(
                                $"{(type == "account" ? "accounts" : "characters")}:{username.ToLower()}"))
                            {
                                WriteLine("User does not exist.");
                                continue;
                            }
                            var account =
                                Server.CacheClient.Database.StringGet(
                                    $"{(type == "account" ? "accounts" : "characters")}:{username.ToLower()}");
                            WriteLine(account);
                            if (cmdArgs.Length >= 3)
                                Clipboard.SetText(account);
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
                            Server.CacheClient.Add($"accounts:{username.ToLower()}", account);
                            WriteLine("Success!");
                            continue;
                        }

                        WriteLine("Invalid Arguments.");
                        break;
                    case "ban":
                        if (cmdArgs?.Length >= 1)
                        {
                            var username = cmdArgs[0];
                            if (!Server.CacheClient.Exists($"accounts:{username.ToLower()}"))
                            {
                                WriteLine("User does not exist.");
                                continue;
                            }
                            var account =
                                Server.CacheClient.Get<Account>($"accounts:{username.ToLower()}");
                            account.Banned = true;
                            Server.CacheClient.Remove($"accounts:{account.Username.ToLower()}");
                            Server.CacheClient.Add($"accounts:{account.Username.ToLower()}",
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
                            if (!Server.CacheClient.Exists($"accounts:{username.ToLower()}"))
                            {
                                WriteLine("User does not exist.");
                                continue;
                            }
                            var account =
                                Server.CacheClient.Get<Account>($"accounts:{username.ToLower()}");
                            account.Banned = false;
                            Server.CacheClient.Remove($"accounts:{account.Username.ToLower()}");
                            Server.CacheClient.Add($"accounts:{account.Username.ToLower()}",
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
                            if (Server.CacheClient.Exists($"accounts:{username.ToLower()}"))
                            {
                                var account = Server.CacheClient.Get<Account>($"accounts:{username.ToLower()}");
                                foreach (var name in account.Characters)
                                {
                                    Server.CacheClient.Remove($"characters:{name.ToLower()}");
                                }
                                Server.CacheClient.Remove($"accounts:{username.ToLower()}");
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
                                Server.CacheClient.Exists($"accounts:{username.ToLower()}"));
                            continue;
                        }

                        WriteLine("Invalid Arguments.");
                        break;
                    default:
                        WriteLine("Unknown command.");
                        break;
                }
            }
        }
    }
}