using System;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Handlers;

namespace ImaginationServer.World.Handlers.World
{
    class ClientValidationHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, string address)
        {
            var username = reader.ReadWString(66);
            reader.BaseStream.Position = 74;
            var userKey = reader.ReadWString(66);

            // TODO: Verify user key
            LuServer.CurrentServer.Clients[address].Authenticated = true;
            LuServer.CurrentServer.Clients[address].Username = username;
        }
    }
}
