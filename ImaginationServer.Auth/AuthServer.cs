using System;
using System.Threading;
using System.Windows.Forms;
using ImaginationServer.Auth.Handlers.Auth;
using ImaginationServer.Common;
using Newtonsoft.Json;
using static System.Console;
using static ImaginationServer.Common.PacketEnums;
using static ImaginationServer.Common.PacketEnums.ClientAuthPacketId;

namespace ImaginationServer.Auth
{
    public class AuthServer
    {
        public static LuServer Server;

        public static void Init()
        {
            Server = new LuServer(1001, 1000, "127.0.0.1");
            Server.AddHandler((byte) RemoteConnection.Auth, (byte) MsgAuthLoginRequest, new LoginRequestHandler());
            Server.Start();
            new Thread(Command).Start();
        }

        [STAThread]
        public static void Command()
        {
            while (!Environment.HasShutdownStarted)
            {
                var input = ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;
                var split = input.Split(new[] { ' ' }, 2);
                var cmd = split[0].ToLower();
                var cmdArgs = split.Length > 1 ? split[1].Split(' ') : null;
                using (var database = new DbUtils())
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
                                if (!database.CharacterExists(name))
                                {
                                    WriteLine("Character does not exist.");
                                    continue;
                                }

                                database.DeleteCharacter(database.GetCharacter(name));

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
                                if (!database.AccountExists(username))
                                {
                                    WriteLine("Account/character does not exist.");
                                    continue;
                                }
                                var account =
                                    JsonConvert.SerializeObject(
                                        (type == "a"
                                            ? (dynamic) database.GetAccount(username)
                                            : (dynamic) database.GetCharacter(username)), Formatting.Indented);
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
                                database.CreateAccount(username, password);

                                WriteLine("Success!");
                                continue;
                            }

                            WriteLine("Invalid Arguments.");
                            break;
                        case "ban":
                            if (cmdArgs?.Length >= 1)
                            {
                                var username = cmdArgs[0];
                                if (!database.AccountExists(username))
                                {
                                    WriteLine("User does not exist.");
                                    continue;
                                }
                                var account = database.GetAccount(username);
                                account.Banned = true;
                                database.UpdateAccount(account);
                                WriteLine("Success!");
                                continue;
                            }

                            WriteLine("Invalid Arguments.");
                            break;
                        case "unban":
                            if (cmdArgs?.Length >= 1)
                            {
                                var username = cmdArgs[0];
                                if (!database.AccountExists(username))
                                {
                                    WriteLine("User does not exist.");
                                    continue;
                                }
                                var account = database.GetAccount(username);
                                account.Banned = false;
                                database.UpdateAccount(account);
                                WriteLine("Success!");
                                continue;
                            }

                            WriteLine("Invalid Arguments.");
                            break;
                        case "removeaccount":
                            if (cmdArgs?.Length >= 1)
                            {
                                var username = cmdArgs[0];
                                if (database.AccountExists(username))
                                {
                                    database.DeleteAccount(database.GetAccount(username));
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
                                WriteLine(database.AccountExists(username));
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

        public static void Service()
        {
            Server.Service();
        }
    }
}