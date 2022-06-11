using System;
using System.Collections.Generic;

namespace MiniFacebook.Data
{
    public partial class Image
    {
        public Image()
        {
            Posts = new HashSet<Post>();
        }

        public long Id { get; set; }
        public byte[] ImageData { get; set; } = null!;

        public virtual ICollection<Post> Posts { get; set; }
    }
}
