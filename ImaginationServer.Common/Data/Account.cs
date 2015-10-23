using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ImaginationServer.Common.Data
{
    public class Account
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public bool Banned { get; set; }
        public DateTime Created { get; set; }
        public List<string> Characters { get; set; }

        public static Account Create(string username, string password)
        {
            if (!LuServer.CurrentServer.CacheClient.Database.KeyExists("NextAccountId"))
            {
                LuServer.CurrentServer.CacheClient.Database.StringSet("NextAccountId", "1");
            }

            var id = long.Parse(LuServer.CurrentServer.CacheClient.Database.StringGet("NextAccountId"));

            var passwordSalt = new byte[64];
            new RNGCryptoServiceProvider().GetBytes(passwordSalt);
            var passwordHash =
                SHA512.Create().ComputeHash(Encoding.Unicode.GetBytes(password).Concat(passwordSalt).ToArray());
            var account = new Account
            {
                Id = id,
                Username = username,
                Salt = passwordSalt,
                Password = passwordHash,
                Banned = false,
                Created = DateTime.Now,
                Characters = new List<string>(4)
            };

            LuServer.CurrentServer.CacheClient.Database.StringSet("NextAccountId", (id + 1).ToString());

            return account;
        }
    }
}