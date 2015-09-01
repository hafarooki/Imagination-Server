using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ImaginationServer.Common.Packets;

namespace ImaginationServer.Auth.Packets.Auth
{
    public class LoginRequest : IncomingPacket
    {
        public string Username { get; }
        public string Password { get; }

        public LoginRequest(BinaryReader reader) : base(reader)
        {
            var usernameChars = new List<char>();
            while (true)
            {
                var c = reader.ReadChar();
                if (c == '\0' || usernameChars.Count >= 30) break;
                usernameChars.Add(c);
            }
            Username = new string(usernameChars.ToArray());
            reader.BaseStream.Position = 74; // password starts at 74
            var passwordChars = new List<char>();
            while (true)
            {
                var c = reader.ReadChar();
                if (c == '\0') break;
                passwordChars.Add(c);
            }
            Password = new string(passwordChars.ToArray());
            Console.WriteLine(Username + " sent authentication request.");
        }
    }
}