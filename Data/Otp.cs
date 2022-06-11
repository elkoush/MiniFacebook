using System;
using System.Collections.Generic;

namespace MiniFacebook.Data
{
    public partial class Otp
    {
        public long Id { get; set; }
        public string Code { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public string Mobile { get; set; } = null!;
        public short VerificationCount { get; set; }
    }
}
