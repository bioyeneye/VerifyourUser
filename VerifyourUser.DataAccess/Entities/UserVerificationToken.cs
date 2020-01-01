using System;
using System.Collections.Generic;
using System.Text;

namespace VerifyourUser.DataAccess.Entities
{
    public class UserVerificationToken
    {
        public string Id { get; set; }
        public UserVerificationTokenType UserVerificationTokenType { get; set; }
        public string Token { get; set; }
        public bool TokenUsed { get; set; }
        public User User { get; set; }
    }

    public enum UserVerificationTokenType
    {
        PHONENUMBER,
        EMAIL
    }
}
