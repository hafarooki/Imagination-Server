using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ImaginationServer.Auth.Packets.Auth;
using ImaginationServer.Common;
using ImaginationServer.Common.Data;
using ImaginationServer.Common.Handlers;

namespace ImaginationServer.Auth.Handlers.Auth
{
    public class LoginRequestHandler : PacketHandler
    {
        public override void Handle(byte[] data, string address)
        {
            var loginRequest = new LoginRequest(data);

            var valid = LuServer.CurrentServer.CacheClient.Exists("accounts:" + loginRequest.Username);
            if (valid)
            {
                var account = LuServer.CurrentServer.CacheClient.Get<Account>("accounts:" + loginRequest.Username);
                var hash =
                    SHA512.Create()
                        .ComputeHash(Encoding.Unicode.GetBytes(loginRequest.Password).Concat(account.Salt).ToArray());
                valid = account.Password == hash;
            }

            // TODO: Respond
        }
    }
}