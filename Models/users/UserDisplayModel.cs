namespace MiniFacebook.Models.users
{
    public class UserDisplayModel
    {
        public string Username { get; set; }
        public string Mobile { get; set; } = null!;
        public bool MobileConfirmed { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
