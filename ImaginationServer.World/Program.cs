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
                Console.WriteLine("Starting Imagination Server World");
                var server = new LuServer(ServerId.World, 2006, 1000, "127.0.0.1");
                server.Handlers.Add(
                    new Tuple<ushort, uint>((ushort) PacketEnums.RemoteConnection.World,
                        (uint) PacketEnums.ClientWorldPacketId.MsgWorldClientValidation), new ClientValidationHandler());
                server.Handlers.Add(
                    new Tuple<ushort, uint>((ushort) PacketEnums.RemoteConnection.World,
                        (uint) PacketEnums.ClientWorldPacketId.MsgWorldClientLoginRequest),
                    new ClientLoginRequestHandler());
                server.Handlers.Add(
                    new Tuple<ushort, uint>((ushort) PacketEnums.RemoteConnection.World,
                        (uint) PacketEnums.ClientWorldPacketId.MsgWorldClientCharacterListRequest),
                    new CharacterListRequestHandler());
                server.Handlers.Add(
                    new Tuple<ushort, uint>((ushort) PacketEnums.RemoteConnection.World,
                        (uint) PacketEnums.ClientWorldPacketId.MsgWorldClientCharacterCreateRequest),
                    new ClientCharacterCreateRequestHandler());
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