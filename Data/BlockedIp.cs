using System;
using System.Collections.Generic;

namespace MiniFacebook.Data
{
    public partial class BlockedIp
    {
        public long Id { get; set; }
        public string Ip { get; set; } = null!;
        public short FailedAttempts { get; set; }
        public bool Blocked { get; set; }
    }
}
