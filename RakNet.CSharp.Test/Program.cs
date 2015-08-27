using RakNetCSharp;

namespace RakNet.CSharp.Test
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            using (var bitStream = new WBitStream())
            {
                sbyte[] chars = "test".ToCharArray();
                bitStream.Write(chars);
            }
        }
    }
}
