using System;
using System.Collections.Generic;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.CharacterData;
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
        public override void Handle(BinaryReader reader, LuClient client)
        {
            using (var database = new DbUtils())
            {
                if (!client.Authenticated)
                    return; // You need to have an account and be signed into it to make a character!

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
                    (byte) (database.CharacterExists(name) ? 0x04 : 0x00);
                // Generate the respond ID

                var account = database.GetAccount(client.Username);

                if (account.Characters.Count >= 4) // Don't want any cheaters getting more than 4!
                {
                    responseId = 0x04;
                }

                if (responseId == 0x00) // Make sure to actually make it, if the character does not exist.
                {
                    // Create the new character
                    var character = new Character
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
                        ShirtStyle = shirtStyle,
                        // Initialize the other character data
                        Position = ZonePositions.VentureExplorer,
                        Owner = client.Username,
                        MapInstance = 0,
                        MapClone = 0,
                        ZoneId = (ushort) ZoneId.VentureExplorer,
                        Armor = 0,
                        MaxArmor = 0,
                        Health = 4,
                        MaxHealth = 4,
                        Imagination = 0,
                        MaxImagination = 0,
                        GmLevel = 0,
                        Reputation = 0,
                        Items = new List<BackpackItem>(),
                        BackpackSpace = 20,
                        Level = 0,
                        Missions = new List<string>()
                    };

                    character.Items.Add(
                        new BackpackItem
                        {
                            Lot = WorldPackets.FindCharShirtID(shirtColor, shirtStyle),
                            Linked = false,
                            Count = 1,
                            Slot = 0
                        });
                    //character.AddItem(WorldPackets.);

                    database.AddCharacter(character); // Add the character to the database.

                    account.Characters.Add(character.Name); // Add the character to the account
                    database.UpdateAccount(account); // Update the account
                }

                // Output the code
                Console.WriteLine($"Got character create request from {client.Username}. Response Code: {responseId}");

                // Create the response
                using (var bitStream = new WBitStream())
                {
                    bitStream.WriteHeader(RemoteConnection.Client, (uint) MsgClientCharacterCreateResponse);
                    // Always write the packet header.
                    bitStream.Write((responseId)); // Write the response code.
                    WorldServer.Server.Send(bitStream, SystemPriority,
                        ReliableOrdered, 0, client.Address, false); // Send the response.
                }

                if (responseId == 0x00)
                    WorldPackets.SendCharacterListResponse(client.Address, database.GetAccount(client.Username),
                        WorldServer.Server);
                // Send the updated character list.
            }
        }
    }
}