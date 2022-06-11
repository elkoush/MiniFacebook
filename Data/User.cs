using System;
using System.Collections.Generic;

namespace MiniFacebook.Data
{
    public partial class User
    {
        public User()
        {
            FriendFriendNavigations = new HashSet<Friend>();
            FriendUsers = new HashSet<Friend>();
            Posts = new HashSet<Post>();
        }

        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public bool MobileConfirmed { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Ip { get; set; } = null!;

        public virtual ICollection<Friend> FriendFriendNavigations { get; set; }
        public virtual ICollection<Friend> FriendUsers { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
