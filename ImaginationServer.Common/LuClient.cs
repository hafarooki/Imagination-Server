namespace ImaginationServer.Common
{
    public class LuClient
    {
        public LuClient(string address)
        {
            Address = address;
        }

        public string Address { get; set; }
        public bool Authenticated { get; set; }
        public string Username { get; set; }
        public string Character { get; set; }
    }
}