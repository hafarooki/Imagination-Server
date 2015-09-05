using System;
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
                LuServer.CurrentServer.CacheClient.Get<Account>("accounts:" + loginRequest.Username.ToLower()).Banned)
                valid = 0x02;

            WriteLine(LuServer.CurrentServer.CacheClient.Get<Account>("accounts:" + loginRequest.Username.ToLower()).Banned);

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
                    WriteLine("FATAL: Magically, the valid variable was not 0x01, 0x06, or 0x02! (Like, how is that even possible..?)");
                    break;
            }

            WriteLine("User login " + message);

            var userKey = new byte[66];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(userKey);
            }

            if (valid == 0x01)
            {
            }

            SendLoginResponse(address, valid, Encoding.ASCII.GetString(userKey));
        }
    }
}