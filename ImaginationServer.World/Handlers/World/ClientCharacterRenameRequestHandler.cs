using System;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Handlers;
using static ImaginationServer.Common.PacketEnums;
using static ImaginationServer.Common.PacketEnums.WorldServerPacketId;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientCharacterRenameRequestHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, LuClient client)
        {
            using (var database = new DbUtils())
            {
                // Read packet
                var objectId = reader.ReadInt64();
                var newName = reader.ReadWString(66);

                // Gather info
                var account = database.GetAccount(client.Username);
                var character = database.GetCharacter(objectId);

                Console.WriteLine(
                    $"Got character rename request from {client.Username}. Old name: {character.Minifig.Name}. New name: {newName}");

                using (var bitStream = new WBitStream()) // Create packet
                {
                    // Always write packet header
                    bitStream.WriteHeader(RemoteConnection.Client, (uint) MsgClientCharacterRenameResponse);

                    // Make sure they own the accounta
                    if (
                        !string.Equals(account.Username, character.Minifig.Name,
                            StringComparison.CurrentCultureIgnoreCase))
                    {
                        Console.WriteLine("Failed to rename character: You can't rename someone else!");
                        bitStream.Write((byte) 0x01); // Fail code
                    }
                    else if (database.CharacterExists(newName)) // Make sure nobody already has that name
                    {
                        bitStream.Write((byte) 0x03); // Code for username taken
                        Console.WriteLine("Failed to rename character: Name already taken.");
                    }
                    else // Good to go!
                    {
                        try
                        {
                            character.Minifig.Name = newName; // Set their new name
                            database.UpdateCharacter(character); // Update the character
                            bitStream.Write((byte) 0x00); // Success code, everything worked just fine.
                            Console.WriteLine("Successfully renamed character!");
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine($"Error while trying to rename user - {exception}");
                            bitStream.Write(0x01); // Some error?
                        }
                    }

                    // Send the packet
                    WorldServer.Server.Send(bitStream, WPacketPriority.SystemPriority,
                        WPacketReliability.ReliableOrdered, 0, client.Address, false);
                }
            }
        }
    }
}