using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ImaginationServer.Common.Codes;
using ImaginationServer.Common.Handlers;
using ImaginationServer.Common.Handlers.Server;
using ImaginationServer.Common.Subscribers;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;
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
        /// Current instance of the serveer
        /// </summary>
        public static LuServer CurrentServer { get; private set; }
        /// <summary>
        /// A map of IP addresses and client data
        /// </summary>
        public Dictionary<string, LuClient> Clients { get; private set; }
        /// <summary>
        /// A map of packet handlers
        /// </summary>
        public Dictionary<Tuple<ushort, uint>, PacketHandler> Handlers { get; }
        /// <summary>
        /// A map of server to server packet subscribers
        /// </summary>
        public Dictionary<ushort, PacketSubscriber> Subscribers { get; }

        /// <summary>
        /// The ID of the server
        /// </summary>
        public ServerId ServerId { get; }

        /// <summary>
        /// The CacheClient, used for storing json of objects
        /// </summary>
        public StackExchangeRedisCacheClient CacheClient { get; }
        /// <summary>
        /// The ConnectionMultiplexer
        /// </summary>
        public ConnectionMultiplexer Multiplexer { get; }

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="serverId">ID of the server</param>
        /// <param name="port">Port to host on</param>
        /// <param name="maxConnections">Maximum clients that cna connect</param>
        /// <param name="address">Address to host on</param>
        public LuServer(ServerId serverId, int port, int maxConnections, string address) : base(port, maxConnections, address)
        {
            Console.Title = $"Imagination Server - {serverId}";
            Console.WriteLine($"Starting Imagination Server {serverId}");

            CurrentServer = this;

            Handlers = new Dictionary<Tuple<ushort, uint>, PacketHandler>();
            ServerId = serverId;
            Subscribers = new Dictionary<ushort, PacketSubscriber>();

            AddHandler((ushort)RemoteConnection.Server, (uint)MsgServerVersionConfirm, new ConfirmVersionHandler());
            Multiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
            CacheClient = new StackExchangeRedisCacheClient(Multiplexer, new NewtonsoftSerializer());
            Directory.CreateDirectory("Temp");

            //Multiplexer.GetSubscriber().Subscribe("Kill", (channel, value) =>
            //{
            //    Stop();
            //    Environment.Exit((int)value);
            //});
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

        public void AddHandler(ushort remoteConnection, uint packetCode, PacketHandler handler)
        {
            Handlers.Add(new Tuple<ushort, uint>(remoteConnection, packetCode), handler);
        }

        public void AddSubscriber(PacketSubscriber subscriber)
        {
            Multiplexer.GetSubscriber().Subscribe(((ushort)subscriber.Code).ToString(), (channel, value) => subscriber.MessageReceived(value));
        }

        public void WriteHeader(ServerId target, BinaryWriter writer)
        {
            writer.Write((ushort)target);
            writer.Write((ushort)ServerId);
        }

        public void Publish(OpCode code, byte[] data)
        {
            Multiplexer.GetSubscriber().Publish(((ushort)code).ToString(), data);
        }
    }
}