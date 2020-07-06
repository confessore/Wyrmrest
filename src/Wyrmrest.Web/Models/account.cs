namespace Wyrmrest.Web.Models
{
    public class account
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string SHA_Pass_Hash { get; set; }
        public string SessionKey { get; set; }
        public string V { get; set; }
        public string S { get; set; }
        public byte Expansion { get; set; }
    }
}
