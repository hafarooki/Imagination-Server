using System;
using System.IO;
using ImaginationServer.Common;
using ImaginationServer.Common.Handlers;

namespace ImaginationServer.World.Handlers.World
{
    public class ClientRoutePacketHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, LuClient client)
        {
            var subPacketLength = reader.ReadUInt32();
            var remoteConnection = (PacketEnums.RemoteConnection) reader.ReadUInt16();
            var packetId = reader.ReadUInt32();
            reader.ReadByte();
            Console.WriteLine($"Received route packet. Length = {subPacketLength}, RemoteConnection = {remoteConnection}, PacketId = {packetId.ToString("X")}");
        }
    }
}