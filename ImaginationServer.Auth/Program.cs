using System;
using System.Threading;
using System.Windows.Forms;
using ImaginationServer.Auth.Handlers.Auth;
using ImaginationServer.Common;
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
                Server = new LuServer(ServerId.Auth, 1001, 1000, "127.0.0.1");
                Server.AddHandler((byte) RemoteConnection.Auth, (byte) MsgAuthLoginRequest, new LoginRequestHandler());
                if (!Server.Multiplexer.GetDatabase().KeyExists("LastUserId"))
                {
                    Server.Multiplexer.GetDatabase().StringSet("LastUserId", 0);
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
                        WriteLine("addaccount <username> <password>");
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
                            if (!DbUtils.CharacterExists(name))
                            {
                                WriteLine("Character does not exist.");
                                continue;
                            }

                            DbUtils.DeleteCharacter(DbUtils.GetCharacter(name));

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
                        if (cmdArgs != null && cmdArgs.Length >= 2)
                        {
                            var username = cmdArgs[0];
                            var password = cmdArgs[1];
                            DbUtils.CreateAccount(username, password);
                            
                            WriteLine("Success!");
                            continue;
                        }

                        WriteLine("Invalid Arguments.");
                        break;
                    case "ban":
                        if (cmdArgs?.Length >= 1)
                        {
                            var username = cmdArgs[0];
                            if (!DbUtils.AccountExists(username))
                            {
                                WriteLine("User does not exist.");
                                continue;
                            }
                            var account = DbUtils.GetAccount(username);
                            account.Banned = true;
                            DbUtils.UpdateAccount(account);
                            WriteLine("Success!");
                            continue;
                        }

                        WriteLine("Invalid Arguments.");
                        break;
                    case "unban":
                        if (cmdArgs?.Length >= 1)
                        {
                            var username = cmdArgs[0];
                            if (!DbUtils.AccountExists(username))
                            {
                                WriteLine("User does not exist.");
                                continue;
                            }
                            var account = DbUtils.GetAccount(username);
                            account.Banned = false;
                            DbUtils.UpdateAccount(account);
                            WriteLine("Success!");
                            continue;
                        }

                        WriteLine("Invalid Arguments.");
                        break;
                    case "removeaccount":
                        if (cmdArgs?.Length >= 1)
                        {
                            var username = cmdArgs[0];
                            if (DbUtils.AccountExists(username))
                            {
                                DbUtils.DeleteAccount(DbUtils.GetAccount(username));
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
                            WriteLine(DbUtils.AccountExists(username));
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