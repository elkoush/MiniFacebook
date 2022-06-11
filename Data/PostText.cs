using System;
using System.Collections.Generic;

namespace MiniFacebook.Data
{
    public partial class PostText
    {
        public PostText()
        {
            Posts = new HashSet<Post>();
        }

        public long Id { get; set; }
        public string Text { get; set; } = null!;

        public virtual ICollection<Post> Posts { get; set; }
    }
}
