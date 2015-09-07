using System;
using System.IO;
using System.Linq;
using ImaginationServer.Common;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Handlers;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientLoginRequestHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, string address)
        {
            var objectId = reader.ReadUInt64();
            Console.WriteLine("Received Login Request from {0} - ObjectID = {1}", address, objectId);

            var client = LuServer.CurrentServer.Clients[address];
            var account = LuServer.CurrentServer.CacheClient.Get<Account>("accounts:" + client.Username.ToLower());

            var index = -1;

            foreach (var characterName in from characterName in account.Characters let character = LuServer.CurrentServer.CacheClient.Get<Character>("characters:" + characterName.ToLower()) where character.Id == objectId select characterName)
            {
                index = account.Characters.IndexOf(characterName);
            }

            if (index == -1)
            {
                Console.WriteLine("USER SENT OBJECT ID THAT IS NOT ONE OF THEIR CHARACTER'S!!!");
                // TODO: Kick user
                return;
            }

            var characterSelected = LuServer.CurrentServer.CacheClient.Get<Character>("characters:" + account.Characters[index].ToLower());

            Console.WriteLine("User has selected character {0}. Sending them to zone {1}.", characterSelected.Minifig.Name, characterSelected.ZoneId);

            using (var bitStream = new WBitStream())
            {
                bitStream.Write((byte)83);
                bitStream.Write((ushort)5);
                bitStream.Write((uint)PacketEnums.WorldServerPacketId.MsgClientTransferToWorld);
                bitStream.Write((byte)0);
                bitStream.WriteString("127.0.0.1", 33);
                bitStream.Write(ResolvePortFromZone((ZoneId) characterSelected.ZoneId));
                bitStream.Write((byte)0);

                LuServer.CurrentServer.Send(bitStream, WPacketPriority.SystemPriority, WPacketReliability.ReliableOrdered, 0, address, false);
            }
        }

        private ushort ResolvePortFromZone(ZoneId zone)
        {
            return (ushort) (2006 + zone);
        }
    }
}