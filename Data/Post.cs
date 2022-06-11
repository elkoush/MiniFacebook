using System;
using System.Collections.Generic;

namespace MiniFacebook.Data
{
    public partial class Post
    {
        public Post()
        {
            InverseParent = new HashSet<Post>();
        }

        public long Id { get; set; }
        public long UserId { get; set; }
        public long? PostTextId { get; set; }
        public long? ImageId { get; set; }
        public long? ParentId { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Image? Image { get; set; }
        public virtual Post? Parent { get; set; }
        public virtual PostText? PostText { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Post> InverseParent { get; set; }
    }
}
