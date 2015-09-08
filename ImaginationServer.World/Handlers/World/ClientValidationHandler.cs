using System;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Handlers;

namespace ImaginationServer.World.Handlers.World
{
    class ClientValidationHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, string address)
        {
            var username = reader.ReadWString(66);
            reader.BaseStream.Position = 74;
            var userKey = reader.ReadWString(66);

            LuServer.CurrentServer.Clients[address].Authenticated = true;
            LuServer.CurrentServer.Clients[address].Username = username;
            // TODO: Verify user key

            if (LuServer.CurrentServer.ServerId.HasFlag(ServerId.Character)) return;

            var characterName =
                LuServer.CurrentServer.CacheClient.Database.StringGet("cache:selectedcharacter:" + username.ToLower());
            var character = LuServer.CurrentServer.CacheClient.Get<Character>("characters:" + characterName);

            using (var bitStream = new WBitStream())
            {
                bitStream.Write((byte)83);
                bitStream.Write((ushort)5);
                bitStream.Write((uint)PacketEnums.WorldServerPacketId.MsgClientLoadStaticZone);
                bitStream.Write((byte)0);

                bitStream.Write(character.ZoneId);
                bitStream.Write(character.MapInstance);
                bitStream.Write(character.MapClone);
                for(var i = 0; i < 4; i++) bitStream.Write(ZoneChecksums.Checksums[(ZoneId) character.ZoneId][i]);
                for(var i = 0; i < 3; i++) bitStream.Write(character.Position[i]);
                bitStream.Write((uint)0);

                LuServer.CurrentServer.Send(bitStream, WPacketPriority.SystemPriority, WPacketReliability.ReliableOrdered, 0, address, false);
            }
        }
    }
}
