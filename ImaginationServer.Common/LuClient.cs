namespace ImaginationServer.Common
{
    public class LuClient
    {
        public LuClient(string address)
        {
            Address = address;
        }

        public string Address { get; set; }
    }
}