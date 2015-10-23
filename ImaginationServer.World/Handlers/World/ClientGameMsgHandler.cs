using System;
using System.Collections.Generic;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Handlers;
using ImaginationServer.World.Handlers.World.GameMsgHandlers;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientGameMsgHandler : PacketHandler
    {
        private Dictionary<ushort, GameMsgHandler> _handlers;

        public ClientGameMsgHandler()
        {
            _handlers = new Dictionary<ushort, GameMsgHandler>();
            if (LuServer.LogUnknownPackets)
            {
                Directory.CreateDirectory("Packets/Game Messages");
            }
            // TODO: Add handlers
        } 

        public override void Handle(BinaryReader reader, string address)
        {
            var objectId = reader.ReadInt64();
            //var flags = "";
            //reader.BaseStream.Position = reader.BaseStream.Position - 4;
            //for (uint k = 0; k < 4; k++)
            //{
            //    var flag = reader.ReadBoolean();
            //    if (flag)
            //    {
            //        flags += $"[{k}]";
            //    }
            //}

            var messageId = reader.ReadUInt16();

            if (!_handlers.ContainsKey(messageId))
            {
                Console.WriteLine($"Received unhandled client game message. ObjectId = \"{objectId}\", MessageId = \"{messageId}\"");
                if (LuServer.LogUnknownPackets)
                {
                    reader.BaseStream.Position = 0;
                    var bytes = ReadFully(reader.BaseStream);
                    File.WriteAllBytes("Packets/Game Messages/" + objectId + "_" + messageId + ".bin", bytes);
                }
            }
            else
            {
                _handlers[messageId].Handle(objectId, reader);
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}