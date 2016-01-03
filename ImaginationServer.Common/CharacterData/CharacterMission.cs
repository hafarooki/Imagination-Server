using Newtonsoft.Json;

namespace ImaginationServer.Common.CharacterData
{
    public class CharacterMission
    {
        public int Id { get; set; }
        public byte Count { get; set; }
        public long Timestamp { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static CharacterMission FromJson(string json)
        {
            return JsonConvert.DeserializeObject<CharacterMission>(json);
        }
    }
}