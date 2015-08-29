using System;
using System.Collections.Generic;
using ImaginationServer.Common.Handlers;
using ImaginationServer.Common.Handlers.Server;

namespace ImaginationServer.Common
{
    public class LuServer : BaseServer
    {

        public static LuServer CurrentServer { get; private set; }

        public Dictionary<string, LuClient> Clients { get; private set; }
        public Dictionary<Tuple<byte, byte>, PacketHandler> Handlers { get; } 

        public ServerId ServerId { get; }

        public LuServer(ServerId serverId, int port, int maxConnections, string address) : base(port, maxConnections, address)
        {
            CurrentServer = this;

            Handlers = new Dictionary<Tuple<byte, byte>, PacketHandler>();
            ServerId = serverId;

            Handlers.Add(new Tuple<byte, byte>((byte) PacketEnums.RemoteConnection.Server, (byte) PacketEnums.ServerPacketId.MsgServerVersionConfirm), new ConfirmVersionHandler());
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
            var tuple = new Tuple<byte, byte>(data[1], data[3]);

            if (!Handlers.ContainsKey(tuple))
            {
                Console.WriteLine("Unknown packet received! {0}:{1}", data[1], data[3]);
                return;
            }

            Handlers[tuple].Handle(data, address);
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