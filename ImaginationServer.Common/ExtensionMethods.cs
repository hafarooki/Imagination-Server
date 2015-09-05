using System.Collections.Generic;
using System.IO;

namespace ImaginationServer.Common
{
    public static class ExtensionMethods
    {
        public static string ReadWString(this BinaryReader reader, int maxLength)
        {
            var valueChars = new List<char>();
            while (true)
            {
                var c = reader.ReadChar();
                if (c == '\0' || valueChars.Count >= 30) break;
                valueChars.Add(c);
            }
            return new string(valueChars.ToArray());
        }    
    }
}