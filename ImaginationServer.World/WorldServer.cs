using System;
using ImaginationServer.Common;
using ImaginationServer.World.Handlers.World;
using static ImaginationServer.Common.PacketEnums;
using static ImaginationServer.Common.PacketEnums.ClientWorldPacketId;

namespace ImaginationServer.World
{
    public class WorldServer
    {
        public static LuServer Server { get; private set; }

        public static void Init()
        {
            Server = new LuServer(2006, 1000, "127.0.0.1");
            Server.AddHandler((ushort) RemoteConnection.World, (uint) MsgWorldClientValidation,
                new ClientValidationHandler());
            Server.AddHandler((ushort) RemoteConnection.World, (uint) MsgWorldClientLoginRequest,
                new ClientLoginRequestHandler());
            Server.AddHandler((ushort) RemoteConnection.World, (uint) MsgWorldClientCharacterListRequest,
                new CharacterListRequestHandler());
            Server.AddHandler((ushort) RemoteConnection.World, (uint) MsgWorldClientCharacterCreateRequest,
                new ClientCharacterCreateRequestHandler());
            Server.AddHandler((ushort) RemoteConnection.World, (uint) MsgWorldClientCharacterDeleteRequest,
                new ClientCharacterDeleteRequestHandler());
            Server.AddHandler((ushort) RemoteConnection.World, (uint) MsgWorldClientCharacterRenameRequest,
                new ClientCharacterRenameRequestHandler());
            Server.AddHandler((ushort) RemoteConnection.World, (uint) MsgWorldClientLevelLoadComplete,
                new ClientLevelLoadCompleteHandler());
            Server.AddHandler((ushort) RemoteConnection.World, (uint) MsgWorldClientGameMsg,
                new ClientGameMsgHandler());
            Server.AddHandler((ushort) RemoteConnection.World, (uint) MsgWorldClientRoutePacket,
                new ClientRoutePacketHandler());
            Server.Start();
        }

        public static void Service()
        {
            Server.Service();
        }
    }
}