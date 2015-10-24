using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using ImaginationServer.Common;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Handlers;
using static ImaginationServer.Common.PacketEnums;
using static ImaginationServer.Common.PacketEnums.WorldServerPacketId;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientLevelLoadCompleteHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, string address)
        {
            var client = LuServer.CurrentServer.Clients[address];

            if (!client.Authenticated) return;

            var zone = (ZoneId) reader.ReadUInt16();
            var instance = reader.ReadUInt16();
            var clone = reader.ReadInt32();

            Console.WriteLine(
                $"Got clientside level load complete packet from {client.Username}. Zone: {zone}, Instance: {instance}, Clone: {clone}.");

            var account = DbUtils.GetAccount(client.Username);
            var character = DbUtils.GetCharacter(client.Character);

            using (var bitStream = new WBitStream())
            {
                bitStream.WriteHeader(RemoteConnection.Client, (uint) MsgClientCreateCharacter);

                using (var ldf = new Ldf())
                {
                    // TODO: Improve LDF code here
                    ldf.WriteS64("accountID", account.Id);
                    ldf.WriteS32("chatmode", 0);
                    ldf.WriteBool("editor_enabled", false);
                    ldf.WriteS32("editor_level", 0);
                    ldf.WriteBool("freetrial", false);
                    ldf.WriteS32("gmlevel", character.GmLevel);
                    ldf.WriteBool("legoclub", true);
                    var levelid = character.ZoneId + (((long) character.MapInstance) << 16) +
                                  (((long) character.MapClone) << 32);
                    ldf.WriteS64("levelid", levelid);
                    ldf.WriteWString("name", character.Minifig.Name);
                    ldf.WriteId("objid", character.Id);
                    ldf.WriteFloat("position.x", character.Position[0]);
                    ldf.WriteFloat("position.y", character.Position[1]);
                    ldf.WriteFloat("position.z", character.Position[2]);
                    ldf.WriteS64("reputation", character.Reputation);
                    ldf.WriteS32("template", 1);
                    using (var xmlData = GenXmlData(character)) ldf.WriteBytes("xmlData", xmlData);

                    bitStream.Write(ldf.GetSize() + 1);
                    bitStream.Write((byte) 0);
                    ldf.WriteToPacket(bitStream);
                    LuServer.CurrentServer.Send(bitStream, WPacketPriority.SystemPriority,
                        WPacketReliability.ReliableOrdered, 0, address, false);
                    File.WriteAllBytes("Temp/" + character.Minifig.Name + ".world_2a.bin", bitStream.GetBytes());
                }
            }

            LuServer.CurrentServer.SendGameMessage(address, character.Id, 1642);
            LuServer.CurrentServer.SendGameMessage(address, character.Id, 509);
            using (var gameMessage = LuServer.CreateGameMessage(character.Id, 472))
            {
                gameMessage.Write((uint) 185);
                gameMessage.Write((byte) 0);
                LuServer.CurrentServer.Send(gameMessage, WPacketPriority.SystemPriority,
                    WPacketReliability.ReliableOrdered, 0, address, false);
            }
        }

        private static WBitStream GenXmlData(Character character)
        {
            using (var str = new StringWriter())
            using (var writer = new XmlTextWriter(str))
            {
                writer.WriteStartDocument(); // <xml>
                writer.WriteStartElement("obj"); // <obj>
                writer.WriteAttributeString("v", "1"); // id="1"

                writer.WriteStartElement("buff"); // <buff>
                writer.WriteEndElement(); // </buff>

                writer.WriteStartElement("skil"); // <skil>
                writer.WriteEndElement(); // </skil>

                writer.WriteStartElement("inv"); // <inv>

                writer.WriteStartElement("bag"); // <bag>
                writer.WriteStartElement("b"); // <b>
                writer.WriteAttributeString("t", "0"); // t="0"
                writer.WriteAttributeString("m", "24"); // m="24"
                writer.WriteEndElement(); // </b>
                writer.WriteEndElement(); // </bag>

                writer.WriteStartElement("items"); // <items>
                writer.WriteStartElement("in"); // <in>

                // TODO: Write items

                //foreach (var item in character.Items)
                //{
                //    writer.WriteStartElement("i"); // <i>
                //    writer.WriteAttributeString("l", item.);
                //    writer.WriteEndElement(); // </i>
                //}

                writer.WriteEndElement(); // </in>
                writer.WriteEndElement(); // </items>

                writer.WriteEndElement(); // </inv>

                writer.WriteStartElement("mf"); // <mf>
                writer.WriteEndElement(); // </mf>

                writer.WriteStartElement("char"); // <char>
                writer.WriteAttributeString("cc", "100"); // cc="100"
                writer.WriteEndElement(); // </char>

                writer.WriteStartElement("lvl"); // <lvl>
                writer.WriteAttributeString("l", character.Level.ToString()); // l="<character's level?>"
                writer.WriteEndElement(); // </lvl>

                writer.WriteStartElement("flag"); // <flag> 
                writer.WriteEndElement(); // </flag>

                writer.WriteStartElement("pet"); // <pet> 
                writer.WriteEndElement(); // </pet>

                if (character.Missions?.Any() ?? false)
                {
                    writer.WriteStartElement("mis"); // <mis>
                    writer.WriteStartElement("done"); // <done>
                    foreach (var mission in character.Missions)
                    {
                        writer.WriteStartElement("m"); // <m>
                        writer.WriteAttributeString("id", mission.Id.ToString()); // id="<id>"
                        writer.WriteAttributeString("cts", mission.Timestamp.ToString()); // cts="<timestamp>"
                        writer.WriteAttributeString("cct", mission.Count.ToString()); // cct="<count>"
                        writer.WriteEndElement(); // </m>
                    }
                    writer.WriteEndElement(); // </done>
                    writer.WriteEndElement(); // </mis>
                }

                writer.WriteStartElement("mnt"); // <mnt> 
                writer.WriteEndElement(); // </mnt>

                writer.WriteStartElement("dest"); // <dest> 
                writer.WriteEndElement(); // </dest>

                writer.WriteEndElement(); // </obj>
                writer.WriteEndDocument(); // ends document
                var bitStream = new WBitStream();
                bitStream.WriteChars(str.ToString());

                XElement.Parse(str.ToString()).Save("Temp/" + character.Minifig.Name + ".xmldata.xml");

                return bitStream;
            }
        }
    }
}