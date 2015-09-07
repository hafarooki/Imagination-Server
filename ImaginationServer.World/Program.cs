using System;
using ImaginationServer.Common;
using ImaginationServer.World.Handlers.World;

namespace ImaginationServer.World
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                ServerId type;

                if (args.Length < 1)
                {
                    Console.WriteLine("!---FAIL---!");
                    Console.WriteLine("NO SERVER ID");
                    Console.ReadKey(true);
                    return;
                }

                if (!Enum.TryParse(args[0], true, out type))
                {
                    Console.WriteLine("!---FAIL---!");
                    Console.WriteLine("INVALID SERVER ID");
                    Console.ReadKey(true);
                    return;
                }

                Console.WriteLine("Starting Imagination Server World {0}", type);

                type = type | ServerId.World;
                var server = new LuServer(ServerId.World,
                    type.HasFlag(ServerId.Character) ? 2006 : 2006 + (int) (ZoneId)Enum.Parse(typeof (ZoneId), args[0]), 1000,
                    "127.0.0.1");
                server.Handlers.Add(
                    new Tuple<ushort, uint>((ushort) PacketEnums.RemoteConnection.World,
                        (uint) PacketEnums.ClientWorldPacketId.MsgWorldClientValidation), new ClientValidationHandler());
                server.Handlers.Add(
                    new Tuple<ushort, uint>((ushort) PacketEnums.RemoteConnection.World,
                        (uint) PacketEnums.ClientWorldPacketId.MsgWorldClientLoginRequest),
                    new ClientLoginRequestHandler());

                if (type.HasFlag(ServerId.Character))
                {
                    server.Handlers.Add(
                        new Tuple<ushort, uint>((ushort) PacketEnums.RemoteConnection.World,
                            (uint) PacketEnums.ClientWorldPacketId.MsgWorldClientCharacterListRequest),
                        new CharacterListRequestHandler());
                    server.Handlers.Add(
                        new Tuple<ushort, uint>((ushort) PacketEnums.RemoteConnection.World,
                            (uint) PacketEnums.ClientWorldPacketId.MsgWorldClientCharacterCreateRequest),
                        new ClientCharacterCreateRequestHandler());
                }
                Console.WriteLine("->OK");
                server.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey(true);
            }
        }
    }
}