namespace Wyrmrest.Web.Models
{
    public class account_banned
    {
        public int Id { get; set; }
        public int BanDate { get; set; }
        public int UnbanDate { get; set; }
        public string BannedBy { get; set; }
        public string BanReason { get; set; }
        public byte Active { get; set; }
    }
}
