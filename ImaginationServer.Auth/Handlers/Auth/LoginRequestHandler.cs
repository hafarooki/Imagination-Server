using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ImaginationServer.Auth.Packets.Auth;
using ImaginationServer.Common;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Handlers;
using ImaginationServerAuthPackets;

namespace ImaginationServer.Auth.Handlers.Auth
{
    public class LoginRequestHandler : PacketHandler
    {
        public override void Handle(BinaryReader reader, string address)
        {
            var loginRequest = new LoginRequest(reader);

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
                Console.WriteLine(Convert.ToBase64String(account.Password) + ":" + Convert.ToBase64String(hash));
                if (!account.Password.SequenceEqual(hash)) valid = 0x06;
            }

            if (valid == 0x01 &&
                LuServer.CurrentServer.CacheClient.Get<Account>("accounts:" + loginRequest.Username.ToLower()).Banned)
                valid = 0x02;
            var message = "was successful.";
            if (valid == 0x06) message = "failed: invalid credentials.";
            if (valid == 0x02) message = "failed: banned.";
            Console.WriteLine("User login " + message);

            var userKey = new byte[132];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(userKey);
            }
            AuthPackets.SendLoginResponse(address, valid, Encoding.Unicode.GetString(userKey));
        }
    }
}