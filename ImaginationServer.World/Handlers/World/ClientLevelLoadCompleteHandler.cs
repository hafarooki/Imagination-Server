using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using ImaginationServer.Common;
using ImaginationServer.Common.CdClientData;
using ImaginationServer.Common.CharacterData;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Handlers;
using ImaginationServer.World.Replica.Objects;
using static ImaginationServer.Common.PacketEnums;
using static ImaginationServer.Common.PacketEnums.WorldServerPacketId;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientLevelLoadCompleteHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, LuClient client)
        {
            using (var database = new DbUtils())
            {
                if (!client.Authenticated) return;

                var zone = (ZoneId) reader.ReadUInt16();
                var instance = reader.ReadUInt16();
                var clone = reader.ReadInt32();

                Console.WriteLine(
                    $"Got clientside level load complete packet from {client.Username}. Zone: {zone}, Instance: {instance}, Clone: {clone}.");

                var account = database.GetAccount(client.Username);
                var character = database.GetCharacter(account.SelectedCharacter);

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
                        ldf.WriteWString("name", character.Name);
                        ldf.WriteId("objid", Character.GetObjectId(character));
                        ldf.WriteFloat("position.x", character.Position[0]);
                        ldf.WriteFloat("position.y", character.Position[1]);
                        ldf.WriteFloat("position.z", character.Position[2]);
                        ldf.WriteS64("reputation", character.Reputation);
                        ldf.WriteS32("template", 1);
                        using (var xmlData = GenXmlData(character)) ldf.WriteBytes("xmlData", xmlData);

                        bitStream.Write(ldf.GetSize() + 1);
                        bitStream.Write((byte) 0);
                        ldf.WriteToPacket(bitStream);
                        WorldServer.Server.Send(bitStream, WPacketPriority.SystemPriority,
                            WPacketReliability.ReliableOrdered, 0, client.Address, false);
                        File.WriteAllBytes("Temp/" + character.Name + ".world_2a.bin", bitStream.GetBytes());
                    }
                }

                WorldServer.Server.SendGameMessage(client.Address, Character.GetObjectId(character), 1642);
                WorldServer.Server.SendGameMessage(client.Address, Character.GetObjectId(character), 509);
                using (var gameMessage = LuServer.CreateGameMessage(Character.GetObjectId(character), 472))
                {
                    gameMessage.Write((uint) 185);
                    gameMessage.Write((byte) 0);
                    WorldServer.Server.Send(gameMessage, WPacketPriority.SystemPriority,
                        WPacketReliability.ReliableOrdered, 0, client.Address, false);
                }

                var playerObject = new PlayerObject(Character.GetObjectId(character), character.Name);
                playerObject.Construct(WorldServer.Server, client.Address);
            }
        }

        private static WBitStream GenXmlData(Character character)
        {
            using(var cdclient = new CdClientDb())
            {
                var xml = "";
                xml += "<?xml version=\"1.0\"?>";

                xml += "<obj v=\"1\">";
                xml += "<buff/>";
                xml += "<skil/>";

                xml += "<inv>";
                xml += "<bag>";
                xml += "<b t=\"0\" m=\"24\"/>";
                xml += "</bag>";

                xml += "<items>";
                xml += "<in>";

                // TODO: Write items

                //foreach (var item in character.Items)
                //{
                //    writer.WriteStartElement("i"); // <i>
                //    writer.WriteAttributeString("l", item.);
                //    writer.WriteEndElement(); // </i>
                //}

                xml += "</in>";
                xml += "</items>";

                xml += "</inv>";

                xml += "<mf/>";

                xml += "<chars cc=\"100\"></char>";

                xml += $"<lvl l=\"{character.Level}\"/>";

                xml += "<flag/>";
                xml += "<pet/>";

                if (character.Missions?.Any() ?? false)
                {
                    xml += "<mis>";
                    xml += "<done>";
                    xml = character.Missions.Select(mission => CharacterMission.FromJson(mission)).Aggregate(xml, (current, missionData) => current + $"<m id=\"{missionData.Id}\" cts=\"{missionData.Timestamp}\" cct=\"{missionData.Count}\"/>");
                    xml += "</done>";
                    xml += "</mis>";
                }

                xml += "<mnt/>";
                xml += "<dest/>";
                xml += "</obj>";
                var bitStream = new WBitStream();
                Console.WriteLine(xml);
                bitStream.WriteChars(xml);

                return bitStream;
            }
        }
    }
}
