// Respond to requests to delete characters

using System;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Handlers;
using static ImaginationServer.Common.PacketEnums;
using static ImaginationServer.Common.PacketEnums.WorldServerPacketId;
using static WPacketPriority;
using static WPacketReliability;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientCharacterDeleteRequestHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, LuClient client)
        {
            using (var database = new DbUtils())
            {
                if (!client.Authenticated) return;

                var id = reader.ReadInt64(); // The object id of the characte
                Console.WriteLine(id);

                var character = database.GetCharacter(id, true); // Retrieve the character from the database

                Console.WriteLine($"{client.Username} requested to delete their character {character.Name}.");

                using (var bitStream = new WBitStream()) // Create the new bitstream
                {
                    bitStream.WriteHeader(RemoteConnection.Client, (uint) MsgClientDeleteCharacterResponse);
                        // Always write the packet header!
                    if (!string.Equals(character.Owner, client.Username,
                        StringComparison.CurrentCultureIgnoreCase)) // You can't delete someone else's character!
                    {
                        bitStream.Write((byte) 0x02); // Maybe that's the fail code?
                        Console.WriteLine("Failed: Can't delete someone else's character!");
                    }
                    else // Good to go, that's their character, they can delete it if they want.
                    {
                        database.DeleteCharacter(character); // Remove the character from the Redis database
                        bitStream.Write((byte) 0x01); // Success code
                        Console.WriteLine("Successfully deleted character.");
                    }

                    // Send the packet
                    WorldServer.Server.Send(bitStream, SystemPriority, ReliableOrdered, 0, client.Address, false);
                }
            }
        }
    }
}