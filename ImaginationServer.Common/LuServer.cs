using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ImaginationServer.Common.Handlers;
using ImaginationServer.Common.Handlers.Server;
using static ImaginationServer.Common.PacketEnums;
using static ImaginationServer.Common.PacketEnums.ServerPacketId;

namespace ImaginationServer.Common
{
    /// <summary>
    /// Main server class
    /// </summary>
    public class LuServer : BaseServer
    {
        /// <summary>
        /// A map of IP addresses and client data
        /// </summary>
        public Dictionary<string, LuClient> Clients { get; private set; }
        /// <summary>
        /// A map of packet handlers
        /// </summary>
        public Dictionary<Tuple<ushort, uint>, PacketHandler> Handlers { get; }

        public const bool LogUnknownPackets = true;

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="port">Port to host on</param>
        /// <param name="maxConnections">Maximum clients that cna connect</param>
        /// <param name="address">Address to host on</param>
        public LuServer(int port, int maxConnections, string address) : base(port, maxConnections, address)
        {
            Handlers = new Dictionary<Tuple<ushort, uint>, PacketHandler>();
            AddHandler((ushort)RemoteConnection.Server, (uint)MsgServerVersionConfirm, new ConfirmVersionHandler(this, port));
            Directory.CreateDirectory("Temp");

            if (LogUnknownPackets)
            {
                Directory.CreateDirectory("Packets");
            }
        }

        protected override void OnStart()
        {
            Clients = new Dictionary<string, LuClient>();
        }

        protected override void OnStop()
        {
            Clients.Clear();
        }

        protected override void OnReceived(byte[] bytes, uint length, string address)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var reader = new BinaryReader(memoryStream, Encoding.Unicode))
                {
                    reader.ReadByte();
                    var tuple = new Tuple<ushort, uint>(reader.ReadUInt16(), reader.ReadUInt32());

                    if (!Handlers.ContainsKey(tuple))
                    {
                        Console.WriteLine("Unhandled packet received! 53-{0}-00-{1}", tuple.Item1.ToString("X"), tuple.Item2.ToString("X"));
                        if (LogUnknownPackets)
                        {
                            File.WriteAllBytes("Packets/" + tuple.Item1 + "-" + tuple.Item2 + ".bin", bytes);
                        }
                        return;
                    }

                    reader.ReadByte();

                    Handlers[tuple].Handle(reader, Clients[address]);
                }
            }
        }

        protected override void OnDisconnect(string address)
        {
            Clients.Remove(address);
            Console.WriteLine("Client of IP {0} left.", address);
        }

        protected override void OnConnect(string address)
        {
            Clients[address] = new LuClient(address);
            Console.WriteLine("Client of IP {0} joined.", address);
        }

        public void AddHandler(ushort remoteConnection, uint packetCode, PacketHandler handler)
        {
            Handlers.Add(new Tuple<ushort, uint>(remoteConnection, packetCode), handler);
        }

        public void SendGameMessage(string address, long objId, ushort messageId)
        {
            using (var gameMessage = CreateGameMessage(objId, messageId))
            {
                Send(gameMessage, WPacketPriority.SystemPriority, WPacketReliability.ReliableOrdered, 0, address, false);
            }
        }

        public static WBitStream CreateGameMessage(long objId, ushort messageId)
        {
            var gameMessage = new WBitStream();

            gameMessage.Write(objId);
            gameMessage.Write(messageId);

            return gameMessage;
        }
    }
}