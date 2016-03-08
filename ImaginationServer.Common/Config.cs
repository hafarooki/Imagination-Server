using System.IO;
using Newtonsoft.Json;

namespace ImaginationServer.Common
{
    public class Config
    {
        public static Config Current { get; private set; }

        public static void Init()
        {
            if (!File.Exists("config.json"))
            {
                var newConfig = new Config
                {
                    Address = "127.0.0.1",
                    EncryptPackets = true
                };
                var configJson = JsonConvert.SerializeObject(newConfig);
                File.WriteAllText("config.json", configJson);
            }

            Current = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
        }

        public string Address { get; set; }
        public bool EncryptPackets { get; set; }
    }
}
