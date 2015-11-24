using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ImaginationServer.Auth.Packets.Auth;
using ImaginationServer.Common;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Handlers;
using static System.Console;
using static ImaginationServerAuthPackets.AuthPackets;

namespace ImaginationServer.Auth.Handlers.Auth
{
    public class LoginRequestHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, LuClient client)
        {
            var loginRequest = new LoginRequest(reader);
            WriteLine($"{loginRequest.Username} sent authentication request.");

            byte valid = 0x01;
            if (!LuServer.CurrentServer.CacheClient.Exists("accounts:" + loginRequest.Username.ToLower()))
            {
                valid = 0x06;
            }

            if (valid == 0x01)
            {
                var account =
                    LuServer.CurrentServer.CacheClient.Get<Account>("accounts:" + loginRequest.Username.ToLower());
                var hash =
                    SHA512.Create()
                        .ComputeHash(Encoding.Unicode.GetBytes(loginRequest.Password).Concat(account.Salt).ToArray());
                if (!account.Password.SequenceEqual(hash)) valid = 0x06;
            }

            if (valid == 0x01 &&
                LuServer.CurrentServer.CacheClient.Get<Account>($"accounts:{loginRequest.Username.ToLower()}").Banned)
                valid = 0x02;

            var message = "derp";
            switch (valid)
            {
                case 0x01:
                    message = "was successful.";
                    break;
                case 0x06:
                    message = "failed: invalid credentials.";
                    break;
                case 0x02:
                    message = "failed: banned.";
                    break;
                default:
                    WriteLine(
                        "FATAL: Magically, the valid variable was not 0x01, 0x06, or 0x02! (Like, how is that even possible..? I'm only checking because resharper is making me.)");
                    break;
            }

            WriteLine("User login " + message);

            if (valid == 0x01)
            {
                // TODO: Store user key
            }

            // Use C++ auth for now (I hope to eliminate this sometime)
            SendLoginResponse(client.Address, valid, RandomString(66));
            // C# auth, not working atm
/*
            using (var bitStream = new WBitStream())
            {
                bitStream.WriteHeader(PacketEnums.RemoteConnection.Client, 0);

                bitStream.Write(valid);
                bitStream.WriteString("Talk_Like_A_Pirate", 33);
                for (var i = 0; i < 7; i++)
                {
                    bitStream.WriteString("_", 0, 33);
                }
                // client version
                bitStream.Write((ushort)1);
                bitStream.Write((ushort)10);
                bitStream.Write((ushort)64);
                //bitStream.WriteWString(RandomString(32), false, true); // user key
                bitStream.WriteString(RandomString(66), 66, 66);
                bitStream.WriteString("localhost", 33); // redirect ip
                bitStream.WriteString("localhost", 33); // chat ip
                bitStream.Write((ushort)2006); // redirect port
                bitStream.Write((ushort)2003); // chat port
                bitStream.WriteString("localhost", 33); // another ip
                bitStream.WriteString("00000000-0000-0000-0000-000000000000",  37); // possible guid
                bitStream.Write((ushort)0); // zero short

                // localization
                bitStream.Write((byte)0x55);
                bitStream.Write((byte)0x53);
                bitStream.Write((byte)0x00);

                bitStream.Write((byte)0); // first login subscription
                bitStream.Write((byte)0); // subscribed
                bitStream.Write((ulong)0); // zero long
                bitStream.Write((ushort)0); // error message length
                bitStream.WriteString("T", 0, 1); // error message
                bitStream.Write((ushort)324); // extra data length

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

                File.WriteAllBytes("Temp/loginresponse.bin", bitStream.GetBytes());

                LuServer.CurrentServer.Send(bitStream, WPacketPriority.SystemPriority,
                    WPacketReliability.ReliableOrdered, 0, client.Address, false); // Send the packet.
            }
*/
        }

        private static void CreateExtraPacketData(uint stampId, int bracketNum, uint afterNum, WBitStream bitStream)
        {
            const uint zeroPacket = 0;

            bitStream.Write(stampId);
            bitStream.Write(bracketNum);
            bitStream.Write(afterNum);
            bitStream.Write(zeroPacket);
        }

        private string RandomString(int length,
            string allowedChars = "0123456789abcdef")
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length)
                throw new ArgumentException(
                    $"allowedChars may contain no more than {byteSize} characters.");

            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            using (var rng = new RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        // Divide the byte into allowedCharSet-sized groups. If the
                        // random value falls into the last group and the last group is
                        // too small to choose from the entire allowedCharSet, ignore
                        // the value in order to avoid biasing the result.
                        var outOfRangeStart = byteSize - (byteSize%allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i]%allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }
    }
}