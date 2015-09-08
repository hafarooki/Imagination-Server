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
            var client = CurrentServer.Clients[address]; // Get the client from the list of clients.
            if (!client.Authenticated) return; // Make sure they've authenticated first!
            var account = CurrentServer.CacheClient.Get<Account>("accounts:" + client.Username.ToLower());
                // Retrieve their account
            WorldPackets.SendCharacterListResponse(address, account);
                // Call the C++ code that generates and sends the character list packet of the specified account
        }
    }
}