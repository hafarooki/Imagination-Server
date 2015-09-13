using System;
using System.Collections.Generic;
using System.IO;
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
            // TODO: Add handlers
        } 

        public override void Handle(BinaryReader reader, string address)
        {
            var objectId = reader.ReadInt64();
            var flags = "";
            reader.BaseStream.Position = reader.BaseStream.Position - 4;
            for (uint k = 0; k < 4; k++)
            {
                var flag = reader.ReadBoolean();
                if (flag)
                {
                    flags += $"[{k}]";
                }
            }

            var messageId = reader.ReadUInt16();

            if (!_handlers.ContainsKey(messageId))
            {
                Console.WriteLine($"Received unhandled client game message. ObjectId = \"{objectId}\", Flags = \"{flags}\", MessageId = \"{messageId}\"");
            }
            else
            {
                _handlers[messageId].Handle(objectId, flags, reader);
            }
        }
    }
}