namespace MiniFacebook.Models.Post
{
    public class DisplayPostModel
    {
        public Int64 Id { get; set; }
        public Int64 UserId { get; set; }
        public DateTime AddedDate { get; set; }
        public string PostText { get; set; }
        public byte[] PostImage { get; set; }

    }
}
