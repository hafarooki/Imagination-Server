using System;

namespace RakNetTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var bitStream = new WBitStream())
            {
                bitStream.WriteWString("WString", false, false);
                bitStream.WriteString("String", 4);
                bitStream.WriteChars("Chars");
            }
            Console.ReadKey(true);
        }
    }
}
