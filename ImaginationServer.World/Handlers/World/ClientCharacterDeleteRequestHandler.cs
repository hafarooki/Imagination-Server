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
        public override void Handle(BinaryReader reader, string address)
        {
            var id = reader.ReadUInt64(); // The object id of the character
            var character = DbUtils.GetCharacter(id); // Retrieve the character from the database

            using (var bitStream = new WBitStream()) // Create the new bitstream
            {
                bitStream.WriteHeader(RemoteConnection.Client, (uint) MsgClientDeleteCharacterResponse); // Always write the packet header!
                if (!string.Equals(character.Owner, LuServer.CurrentServer.Clients[address].Username,
                        StringComparison.CurrentCultureIgnoreCase)) // You can't delete someone else's character!
                {
                    bitStream.Write((byte) 0x02); // Maybe that's the fail code?
                }
                else // Good to go, that's their character, they can delete it if they want.
                {
                    DbUtils.DeleteCharacter(character); // Remove the character from the Redis database
                    bitStream.Write((byte) 0x01); // Success code
                }

                LuServer.CurrentServer.Send(bitStream, SystemPriority, ReliableOrdered, 0, address, false); // Send the packet
            }
        }
    }
}