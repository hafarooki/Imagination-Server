using ImaginationServer.Common;
using ImaginationServer.Common.Packets;

namespace ImaginationServer.Auth.Packets.Auth
{
    public class LoginResponse : Packet
    {
        public byte Status { get; }

        public LoginResponse(byte status)
        {
            Status = status;
        }

        public override void Serialize(WBitStream bitStream)
        {
            WriteHeader(bitStream, (ushort) PacketEnums.RemoteConnection.Client, (uint) PacketEnums.WorldServerPacketId.MsgClientLoginResponse);

            bitStream.Write((byte) 0x01);
            
            bitStream.WriteString("Talk_Like_A_Pirate", 33);
            for (var i = 0; i < 7; i++) bitStream.WriteString("", 33);
            bitStream.Write((ushort)1);
            bitStream.Write((ushort)10);
            bitStream.Write((ushort)64);
            bitStream.WriteWString("0 9 4 e 7 0 1 a c 3 b 5 5 2 0 b 4 7 8 9 5 b 3 1 8 5 7 b f 1 c 3   ");
            bitStream.WriteString("127.0.0.1", 33);
            bitStream.WriteString("127.0.0.1", 33);
            bitStream.Write((ushort)2006);
            bitStream.Write((ushort)2003);
            bitStream.WriteString("127.0.0.1", 33);
            bitStream.WriteString("00000000-0000-0000-0000-000000000000", 37, 37);
            bitStream.Write((ushort)0);
            bitStream.Write((byte)0x55);
            bitStream.Write((byte)0x53);
            bitStream.Write((byte)0x00);
            bitStream.Write((byte)1);
            bitStream.Write((byte)0);
            bitStream.Write((ulong)0);
            bitStream.Write((ushort)0);
            bitStream.WriteString("T", 0, 1);
            bitStream.Write((uint)324);
            CreateExtraPacketDataSuccess(bitStream);
        }

        private static void CreateExtraPacketDataSuccess(WBitStream bitStream)
        {
            CreateExtraPacketData(0, 0, 2803442767, bitStream);
            CreateExtraPacketData(7, 37381, 2803442767, bitStream);
            CreateExtraPacketData(8, 6, 2803442767, bitStream);
            CreateExtraPacketData(9, 0, 2803442767, bitStream);
            CreateExtraPacketData(10, 0, 2803442767, bitStream);
            CreateExtraPacketData(11, 1, 2803442767, bitStream);
            CreateExtraPacketData(14, 1, 2803442767, bitStream);
            CreateExtraPacketData(15, 0, 2803442767, bitStream);
            CreateExtraPacketData(17, 1, 2803442767, bitStream);
            CreateExtraPacketData(5, 0, 2803442767, bitStream);
            CreateExtraPacketData(6, 1, 2803442767, bitStream);
            CreateExtraPacketData(20, 1, 2803442767, bitStream);
            CreateExtraPacketData(19, 30854, 2803442767, bitStream);
            CreateExtraPacketData(21, 0, 2803442767, bitStream);
            CreateExtraPacketData(22, 0, 2803442767, bitStream);
            CreateExtraPacketData(23, 4114, 2803442767, bitStream);
            CreateExtraPacketData(27, 4114, 2803442767, bitStream);
            CreateExtraPacketData(28, 1, 2803442767, bitStream);
            CreateExtraPacketData(29, 0, 2803442767, bitStream);
            CreateExtraPacketData(30, 30854, 2803442767, bitStream);
        }

        private static void CreateExtraPacketData(uint stampId, int bracketNum, uint afterNum, WBitStream bitStream)
        {
            const uint zeroPacket = 0;

            bitStream.Write(stampId);
            bitStream.Write(bracketNum);
            bitStream.Write(afterNum);
            bitStream.Write(zeroPacket);
        }
    }
}

