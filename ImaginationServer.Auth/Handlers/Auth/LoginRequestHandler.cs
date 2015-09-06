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
        public override void Handle(BinaryReader reader, string address)
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
                        "FATAL: Magically, the valid variable was not 0x01, 0x06, or 0x02! (Like, how is that even possible..?)");
                    break;
            }

            WriteLine("User login " + message);

            if (valid == 0x01)
            {
                // TODO: Store user key
            }

            SendLoginResponse(address, valid, RandomString(66));
        }

        private string RandomString(int length,
            string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
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