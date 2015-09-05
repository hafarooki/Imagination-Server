using System;
using System.IO;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Handlers;
using ImaginationServerWorldPackets;
using static ImaginationServer.Common.LuServer;

namespace ImaginationServer.World.Handlers.World
{
    public class CharacterListRequestHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, string address)
        {
            var client = CurrentServer.Clients[address];
            if (!client.Authenticated) return;
            Console.WriteLine(CurrentServer.Clients[address].Authenticated);
            var account = CurrentServer.CacheClient.Get<Account>("accounts:" + client.Username.ToLower());
            WorldPackets.SendCharacterListResponse(address, (byte) account.Characters.Count);
        }
    }
}