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
            using (var database = new DbUtils())
            {

                var objectId = reader.ReadInt64(); // Read the object ID.
                Console.WriteLine("Received Login Request from {0} - ObjectID = {1}", client.Address, objectId);

                var account = database.GetAccount(client.Username); // Get the account.
                var character = database.GetCharacter(objectId);

                if (!string.Equals(character.Owner, account.Username, StringComparison.CurrentCultureIgnoreCase))
                    // Make sure they selected their own character
                {
                    Console.WriteLine("USER {0} SENT OBJECT ID THAT IS NOT ONE OF THEIR CHARACTER'S!!!", client.Username);
                    // TODO: Kick user
                    return;
                }

                account.SelectedCharacter = character.Name;
                database.UpdateAccount(account);

                client.OutOfChar = true;

                Console.WriteLine("User has selected character {0}. Sending them to zone {1}.", character.Name,
                    character.ZoneId);

                //using (var bitStream = new WBitStream()) // Create the redirect packet
                //{
                //    bitStream.WriteHeader(RemoteConnection.Client, (uint) MsgClientTransferToWorld);
                //    // Always write the header.
                //    bitStream.WriteString("127.0.0.1", 33);
                //    // Write the IP to redirect to (TODO: Make this the broadcast IP)
                //    bitStream.Write(Resol vePortFromZone((ZoneId) character.ZoneId));
                //    // Write the port of the server. (For now, this is 2006 + the zone id)
                //    bitStream.Write((byte) 0);
                //        // Don't say that this was a mythran dimensional shift, because it wasn't.

                //    WorldServer.Server.Send(bitStream, WPacketPriority.SystemPriority,
                //        WPacketReliability.ReliableOrdered, 0, client.Address, false); // Send the redirect packet.
                //}
            }
        }
    }
}