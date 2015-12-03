using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ImaginationServer.Common.Data
{
    public class Account
    {
        public virtual long Id { get; set; }
        public virtual string Username { get; set; }
        public virtual byte[] Password { get; set; }
        public virtual byte[] Salt { get; set; }
        public virtual bool Banned { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual IList<string> Characters { get; set; }
        public virtual string SelectedCharacter { get; set; }

        public static Account Create(string username, string password)
        {
            var passwordSalt = new byte[64];
            new RNGCryptoServiceProvider().GetBytes(passwordSalt);
            var passwordHash =
                SHA512.Create().ComputeHash(Encoding.Unicode.GetBytes(password).Concat(passwordSalt).ToArray());
            var account = new Account
            {
                Username = username,
                Salt = passwordSalt,
                Password = passwordHash,
                Banned = false,
                Created = DateTime.Now,
                Characters = new List<string>(4),
                SelectedCharacter = null
            };

            return account;
        }
    }
}