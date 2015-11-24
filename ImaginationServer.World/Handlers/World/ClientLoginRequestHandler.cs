using System;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Handlers;
using static ImaginationServer.Common.PacketEnums;
using static ImaginationServer.Common.PacketEnums.WorldServerPacketId;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientLoginRequestHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, LuClient client)
        {
            var objectId = reader.ReadInt64(); // Read the object ID.
            Console.WriteLine("Received Login Request from {0} - ObjectID = {1}", client.Address, objectId);

            var account = DbUtils.GetAccount(client.Username); // Get the account.
            var character = DbUtils.GetCharacter(objectId);

            if (!string.Equals(character.Owner, account.Username, StringComparison.CurrentCultureIgnoreCase))
                // Make sure they selected their own character
            {
                Console.WriteLine("USER {0} SENT OBJECT ID THAT IS NOT ONE OF THEIR CHARACTER'S!!!", client.Username);
                // TODO: Kick user
                return;
            }

            // Cache the user's selected character
            LuServer.CurrentServer.CacheClient.Database.StringSet(
                "cache:selectedcharacter:" + client.Username.ToLower(), character.Minifig.Name.ToLower());

            Console.WriteLine("User has selected character {0}. Sending them to zone {1}.", character.Minifig.Name,
                character.ZoneId);

            using (var bitStream = new WBitStream()) // Create the redirect packet
            {
                bitStream.WriteHeader(RemoteConnection.Client, (uint) MsgClientTransferToWorld);
                    // Always write the header.
                bitStream.WriteString("127.0.0.1", 33);
                    // Write the IP to redirect to (TODO: Make this the broadcast IP)
                bitStream.Write(ResolvePortFromZone((ZoneId) character.ZoneId));
                    // Write the port of the server. (For now, this is 2006 + the zone id)
                bitStream.Write((byte) 0); // Don't say that this was a mythran dimensional shift, because it wasn't.

                LuServer.CurrentServer.Send(bitStream, WPacketPriority.SystemPriority,
                    WPacketReliability.ReliableOrdered, 0, client.Address, false); // Send the redirect packet.
            }
        }

        /// <summary>
        /// Resolves the port from the specified zone id.
        /// </summary>
        /// <param name="zone">The zone of the server to retrieve the port of.</param>
        /// <returns>The port of the server that hosts the specified zone.</returns>
        private static ushort ResolvePortFromZone(ZoneId zone)
        {
            return (ushort) (2006 + zone); // Add 2006, the base port, to the zone.
        }
    }
}