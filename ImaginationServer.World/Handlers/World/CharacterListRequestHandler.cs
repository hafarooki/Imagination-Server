using System;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Handlers;
using ImaginationServerWorldPackets;
using Newtonsoft.Json;
using static ImaginationServer.Common.LuServer;

namespace ImaginationServer.World.Handlers.World
{
    public class CharacterListRequestHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, LuClient client)
        {
            if (!client.Authenticated) return; // Make sure they've authenticated first!
            var account = CurrentServer.CacheClient.Get<Account>("accounts:" + client.Username.ToLower());
                // Retrieve their account
            WorldPackets.SendCharacterListResponse(client.Address, account);
                // Call the C++ code that generates and sends the character list packet of the specified account

            Console.WriteLine($"Sent character list packet to {client.Username}. Users: " + JsonConvert.SerializeObject(account.Characters));
        }
    }
}