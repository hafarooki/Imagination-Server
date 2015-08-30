using System;
using System.Collections.Generic;
using ImaginationServer.Common.Handlers;
using ImaginationServer.Common.Handlers.Server;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Protobuf;

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
            Console.Title = "Imagination Server - "  + serverId.ToString();

            CurrentServer = this;

            Handlers = new Dictionary<Tuple<ushort, uint>, PacketHandler>();
            ServerId = serverId;

            Handlers.Add(new Tuple<ushort, uint>((ushort) PacketEnums.RemoteConnection.Server, (uint) PacketEnums.ServerPacketId.MsgServerVersionConfirm), new ConfirmVersionHandler());
            Multiplexer = ConnectionMultiplexer.Connect("127.0.0.1:1500");
            CacheClient = new StackExchangeRedisCacheClient(Multiplexer, new ProtobufSerializer());
        }

        protected override void OnStart()
        {
            Clients = new Dictionary<string, LuClient>();
        }

        protected override void OnStop()
        {
            Clients.Clear();
        }

        protected override void OnReceived(byte[] data, string address)
        {
            using (var bitStream = new WBitStream(data, true))
            {
                bitStream.ReadByte();
                var tuple = new Tuple<ushort, uint>(bitStream.ReadUShort(), bitStream.ReadULong());

                if (!Handlers.ContainsKey(tuple))
                {
                    Console.WriteLine("Unknown packet received! {0}:{1}", tuple.Item1, tuple.Item2);
                    return;
                }

                Handlers[tuple].Handle(data, address);
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