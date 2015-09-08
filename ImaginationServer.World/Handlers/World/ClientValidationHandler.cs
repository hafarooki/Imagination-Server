﻿using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Handlers;
using static ImaginationServer.Common.PacketEnums;
using static ImaginationServer.Common.PacketEnums.WorldServerPacketId;

namespace ImaginationServer.World.Handlers.World
{
    internal class ClientValidationHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, string address)
        {
            var username = reader.ReadWString(66); // Read the username
            reader.BaseStream.Position = 74; // Set the position to 74, to get the user key.
            var userKey = reader.ReadWString(66); // Read the user key

            // Set the user to authenticated
            LuServer.CurrentServer.Clients[address].Authenticated = true;
            LuServer.CurrentServer.Clients[address].Username = username;
            // TODO: Verify user key (Maybe it should expire, instead of just being stored? Otherwise, I'd just cache it.)

            if (LuServer.CurrentServer.ServerId.HasFlag(ServerId.Character))
                return; // Character server doesn't need to do any more.

            var characterName =
                LuServer.CurrentServer.CacheClient.Database.StringGet("cache:selectedcharacter:" + username.ToLower());
                // Get the selected character.
            var character = DbUtils.GetCharacter(characterName);

            using (var bitStream = new WBitStream()) // Create the zone load packet
            {
                bitStream.WriteHeader(RemoteConnection.Client, (uint) MsgClientLoadStaticZone);
                    // Always write the header.

                bitStream.Write(character.ZoneId); // Write the zone id
                bitStream.Write(character.MapInstance); // Write the map instance
                bitStream.Write(character.MapClone); // Write the map clone
                for (var i = 0; i < 4; i++)
                    bitStream.Write(ZoneChecksums.Checksums[(ZoneId) character.ZoneId][i]); // Write the checksum
                bitStream.Write((ushort) 0); // ???
                for (var i = 0; i < 3; i++) bitStream.Write(character.Position[i]); // Write the position
                bitStream.Write((uint) 0); // Supposed to be 4, if in battle...

                // Send the packet
                LuServer.CurrentServer.Send(bitStream, WPacketPriority.SystemPriority,
                    WPacketReliability.ReliableOrdered, 0, address, false);
            }
        }
    }
}