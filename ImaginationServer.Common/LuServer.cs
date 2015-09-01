using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ImaginationServer.Common.Handlers;
using ImaginationServer.Common.Handlers.Server;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace ImaginationServer.Common
{
    public class LuServer : BaseServer
    {

        public static LuServer CurrentServer { get; private set; }

        public Dictionary<string, LuClient> Clients { get; private set; }
        public Dictionary<Tuple<ushort, uint>, PacketHandler> Handlers { get; }

        public ServerId ServerId { get; }

        public StackExchangeRedisCacheClient CacheClient { get; }
        public ConnectionMultiplexer Multiplexer { get; }

        public LuServer(ServerId serverId, int port, int maxConnections, string address) : base(port, maxConnections, address)
        {
            Console.Title = "Imagination Server - " + serverId.ToString();

            CurrentServer = this;

            Handlers = new Dictionary<Tuple<ushort, uint>, PacketHandler>();
            ServerId = serverId;

            Handlers.Add(new Tuple<ushort, uint>((ushort)PacketEnums.RemoteConnection.Server, (uint)PacketEnums.ServerPacketId.MsgServerVersionConfirm), new ConfirmVersionHandler());
            Multiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
            CacheClient = new StackExchangeRedisCacheClient(Multiplexer, new NewtonsoftSerializer());
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
                        Console.WriteLine("Unknown packet received! {0}:{1}", tuple.Item1, tuple.Item2);
                        return;
                    }

                    reader.ReadByte();

                    Handlers[tuple].Handle(reader, address);
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
    }
}