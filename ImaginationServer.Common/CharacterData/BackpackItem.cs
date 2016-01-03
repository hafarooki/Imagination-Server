using Newtonsoft.Json;

namespace ImaginationServer.Common.CharacterData
{
    public class BackpackItem
    {
        public long Lot { get; set; }
        public int Slot { get; set; }
        public int Count { get; set; }
        public bool Linked { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static BackpackItem FromJson(string json)
        {
            return JsonConvert.DeserializeObject<BackpackItem>(json);
        }
    }
}