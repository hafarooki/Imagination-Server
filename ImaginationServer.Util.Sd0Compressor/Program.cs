using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationServer.Util.Sd0Compressor
{
    class Program
    {
        static void Main(string[] args)
        {
            string file;

            if (args.Length > 0) file = args[0];
            else
            {
                Console.Write("File: ");
                file = Console.ReadLine();
            }

            if (file == null || !File.Exists(file)) Environment.Exit(1);

            var input = new FileStream(file, FileMode.Open);
            var output = new FileStream(file + ".sd0", FileMode.Create);

            using (var writer = new BinaryWriter(output, Encoding.UTF8))
            {
                writer.Write('s');
                writer.Write('d');
                writer.Write('0');
                writer.Write((byte)0x01);
                writer.Write((byte)0xff);

                const int chunkMaxLength = 1024 * 256;

                int read;
                var buffer = new byte[chunkMaxLength];

                var chunkNumber = 1;

                while ((read = input.Read(buffer, 0, chunkMaxLength)) > 0)
                {
                    Console.WriteLine("Writing chunk {0}...", chunkNumber);
                    using (var outputStream = new MemoryStream())
                    using (var compressionStream = new DeflateStream(outputStream, CompressionMode.Compress, true))
                    {
                        compressionStream.Write(buffer, 0, read);
                        compressionStream.Flush();
                        compressionStream.Close();

                        writer.Write(outputStream.ToArray().Length);
                        writer.Write(outputStream.ToArray());
                    }
                    Console.WriteLine(" ->OK");
                    chunkNumber++;
                }

                Console.WriteLine("Done.");
            }
        }
    }
}
