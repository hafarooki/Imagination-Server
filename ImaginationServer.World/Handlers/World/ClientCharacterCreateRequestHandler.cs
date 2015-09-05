using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Handlers;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientCharacterCreateRequestHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, string address)
        {
            var client = LuServer.CurrentServer.Clients[address];

            if (!client.Authenticated) return;            

            var name = reader.ReadWString(66);
            reader.BaseStream.Position = 74;
            var name1 = reader.ReadUInt32();
            var name2 = reader.ReadUInt32();
            var name3 = reader.ReadUInt32();

            reader.ReadBytes(9);

            var shirtColor = reader.ReadUInt32();
            var shirtStyle = reader.ReadUInt32();
            var pantsColor = reader.ReadUInt32();
            var hairStyle = reader.ReadUInt32();
            var hairColor = reader.ReadUInt32();
            var lh = reader.ReadUInt32();
            var rh = reader.ReadUInt32();
            var eyebrows = reader.ReadUInt32();
            var eyes = reader.ReadUInt32();
            var mouth = reader.ReadUInt32();

            var character = new Character()
            {
                Minifig = new Minifig()
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
                Position = new float[3],
                Rotation = new float[4]
            };

            var responseId = (byte) ((LuServer.CurrentServer.CacheClient.Exists("characters:" + character.Minifig.Name.ToLower())) ? 0x04 : 0x00);

            if (responseId == 0x00)
            {
                LuServer.CurrentServer.CacheClient.Add("characters:" + character.Minifig.Name.ToLower(), character);

                var account = LuServer.CurrentServer.CacheClient.Get<Account>("accounts:" + client.Username.ToLower());
                account.Characters.Add(character.Minifig.Name);
                LuServer.CurrentServer.CacheClient.Remove("accounts:" + client.Username.ToLower());
                LuServer.CurrentServer.CacheClient.Add("accounts:" + client.Username.ToLower(), account);
            }

            using (var bitStream = new WBitStream())
            {
                bitStream.Write((responseId));
                LuServer.CurrentServer.Send(bitStream, WPacketPriority.SystemPriority, WPacketReliability.ReliableOrdered, 0, address, false);
            }
        }
    }
}