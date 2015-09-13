using System;
using System.Collections.Generic;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Handlers;
using ImaginationServerWorldPackets;
using static ImaginationServer.Common.PacketEnums;
using static ImaginationServer.Common.PacketEnums.WorldServerPacketId;
using static WPacketPriority;
using static WPacketReliability;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientCharacterCreateRequestHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, string address)
        {
            var client = LuServer.CurrentServer.Clients[address]; // Get the client from the address.

            if (!client.Authenticated) return; // You need to have an account and be signed into it to make a character!

            var name = reader.ReadWString(66); // Read the name of the new character
            reader.BaseStream.Position = 74; // Set the position to right after the username
            var name1 = reader.ReadUInt32(); // Read
            var name2 = reader.ReadUInt32(); // FTP
            var name3 = reader.ReadUInt32(); // Names

            // TODO: Implement FTP names

            reader.ReadBytes(9); // Read 9 ... unknown bytes?

            var shirtColor = reader.ReadUInt32(); // Read their choices in appearance
            var shirtStyle = reader.ReadUInt32();
            var pantsColor = reader.ReadUInt32();
            var hairStyle = reader.ReadUInt32();
            var hairColor = reader.ReadUInt32();
            var lh = reader.ReadUInt32();
            var rh = reader.ReadUInt32();
            var eyebrows = reader.ReadUInt32();
            var eyes = reader.ReadUInt32();
            var mouth = reader.ReadUInt32();

            var responseId =
                (byte) ((LuServer.CurrentServer.CacheClient.Exists($"characters:{name.ToLower()}")) ? 0x04 : 0x00);
                // Generate the respond ID

            var account = DbUtils.GetAccount(client.Username);

            if (account.Characters.Count >= 4) // Don't want any cheaters getting more than 4!
            {
                responseId = 0x04;
            }

            if (responseId == 0x00) // Make sure to actually make it, if the character does not exist.
            {
                // Create the new character
                var character = new Character
                {
                    Id =
                        1152921504606846994 +
                        long.Parse(LuServer.CurrentServer.Multiplexer.GetDatabase().StringGet("LastUserId")),
                    // Object ID, starts at 1152921504606846994 (Thanks to Darwin for that number)
                    Minifig = new Minifig // Initialize the minifig customization
                    {
                        Name = name,
                        Eyebrows = eyebrows,
                        Eyes = eyes,
                        HairColor = hairColor,
                        HairStyle = hairStyle,
                        Lh = lh,
                        Rh = rh,
                        Mouth = mouth,
                        Name1 = name1,
                        Name2 = name2,
                        Name3 = name3,
                        PantsColor = pantsColor,
                        ShirtColor = shirtColor,
                        ShirtStyle = shirtStyle
                    },
                    // Initialize the other character data
                    Position = ZonePositions.VentureExplorer,
                    Owner = client.Username,
                    MapInstance = 0,
                    MapClone = 0,
                    ZoneId = (ushort) ZoneId.VentureExplorer,
                    Armor = 2,
                    MaxArmor = 2,
                    Health = 4,
                    MaxHealth = 4,
                    Imagination = 0,
                    MaxImagination = 0,
                    GmLevel = 0,
                    Reputation = 0,

                    Items = new List<BackpackItem>(182)
                };

                character.Items.Add(new BackpackItem(WorldPackets.FindCharShirtID(shirtColor, shirtStyle)));

                DbUtils.AddCharacter(character); // Add the character to the database.

                account.Characters.Add(character.Minifig.Name); // Add the character to the account
                DbUtils.UpdateAccount(account); // Update the account

                var next = long.Parse(LuServer.CurrentServer.Multiplexer.GetDatabase().StringGet("LastUserId"));
                    // Get the last character id
                LuServer.CurrentServer.Multiplexer.GetDatabase()
                    .StringSet(DbUtils.GetIdKey(next), character.Minifig.Name.ToLower()); // Match the name to the id
                next += 1; // Add one to the id, so the next character will have a different id.
                LuServer.CurrentServer.Multiplexer.GetDatabase().StringSet("LastUserId", next);
                    // Update the ID (Luckily, for simple strings, there actually IS a set method!)
            }

            // Output the code
            Console.WriteLine("Got character create request from {0}. Response Code: {1}", client.Username, responseId);

            // Create the response
            using (var bitStream = new WBitStream())
            {
                bitStream.WriteHeader(RemoteConnection.Client, (uint)MsgClientCharacterCreateResponse); // Always write the packet header.
                bitStream.Write((responseId)); // Write the response code.
                LuServer.CurrentServer.Send(bitStream, SystemPriority,
                    ReliableOrdered, 0, address, false); // Send the response.
            }

            if(responseId == 0x00) WorldPackets.SendCharacterListResponse(address, DbUtils.GetAccount(client.Username)); // Send the updated character list.
        }
    }
}