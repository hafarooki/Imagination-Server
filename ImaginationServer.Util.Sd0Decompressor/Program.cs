using System;
using System.IO;
using System.IO.Compression;

namespace ImaginationServer.Util.Sd0Decompressor
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string file;

            if (args.Length > 0) file = args[0];
            else
            {
                Console.Write("File: ");
                file = Console.ReadLine();
            }

            if (file == null || !File.Exists(file)) Environment.Exit(1);

            using (var input = new FileStream(file, FileMode.Open))
            using (
                var output =
                    new FileStream(
                        Path.ChangeExtension(file.Replace(".sd0", string.Empty),
                            ".dec" + Path.GetExtension(file.Replace(".sd0", string.Empty)))
                            .Replace(".sd0", string.Empty), FileMode.Create))
            {
                using (var reader = new BinaryReader(input))
                using (var writer = new BinaryWriter(output))
                {
                    reader.ReadBytes(5); // header

                    var chunkNumber = 1;
                    while (true)
                    {
                        try
                        {
                            var length = reader.ReadInt32();
                            Console.WriteLine("Reading chunk {0}...", chunkNumber);
                            var compressed = reader.ReadBytes(length);
                            using (var ms = new MemoryStream(compressed))
                            using (var deflate = new DeflateStream(ms, CompressionMode.Decompress))
                            {
                                var buffer = new byte[1024 * 256];
                                var read = deflate.Read(buffer, 0, 1024 * 256);
                                writer.Write(buffer, 0, read);
                            }
                            chunkNumber++;
                            Console.WriteLine(" ->OK");
                        }
                        catch (EndOfStreamException)
                        {
                            break;
                        }
                    }
                }
            }

            Console.WriteLine("Done.");
        }
    }
}