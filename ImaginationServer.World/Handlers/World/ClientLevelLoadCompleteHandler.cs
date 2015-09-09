using System;
using System.ComponentModel.Design;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Handlers;
using static ImaginationServer.Common.PacketEnums;
using static ImaginationServer.Common.PacketEnums.WorldServerPacketId;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientLevelLoadCompleteHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, string address)
        {
            var zone = (ZoneId) reader.ReadUInt16();
            var instance = reader.ReadUInt16();
            var clone = reader.ReadInt32();

            var client = LuServer.CurrentServer.Clients[address];

            Console.WriteLine($"Got clientside level load complete packet from {client.Username}. Zone: {zone}, Instance: {instance}, Clone: {clone}.");

            using (var bitStream = new WBitStream())
            {
                bitStream.WriteHeader(RemoteConnection.Client, (uint) MsgClientCreateCharacter);

                var ldf = new Ldf();

                var character = DbUtils.GetCharacter(client.Character);

                // TODO: Improve LDF code here
                ldf.WriteS64("accountID", 0);
                ldf.WriteS32("chatmode", 0);
                ldf.WriteBool("editor_enabled", false);
                ldf.WriteS32("editor_level", 0);
                ldf.WriteBool("freetrial", false);
                ldf.WriteS32("gmlevel", character.GmLevel);
                ldf.WriteBool("legoclub", true);
                ldf.WriteS64("levelid", 0);
                ldf.WriteWString("name", character.Minifig.Name);
                ldf.WriteId("objid", character.Id);
                ldf.WriteFloat("position.x", character.Position[0]);
                ldf.WriteFloat("position.y", character.Position[1]);
                ldf.WriteFloat("position.z", character.Position[2]);
                ldf.WriteS64("reputation", character.Reputation);
                ldf.WriteS32("template", 1);

                bitStream.Write(ldf.GetSize() + 1);
                bitStream.Write((byte)0);
                ldf.WriteToPacket(bitStream);
                LuServer.CurrentServer.Send(bitStream, WPacketPriority.SystemPriority, WPacketReliability.ReliableOrdered, 0, address, false);
            }
        }
    }
}