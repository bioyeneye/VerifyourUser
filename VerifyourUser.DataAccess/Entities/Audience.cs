using System;
using System.Collections.Generic;
using System.Text;

namespace VerifyourUser.DataAccess.Entities
{
    public class Audience
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AppName { get; set; }
        public string Resources { get; set; }
        public string Channels { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public int EmailTokenLength { get; set; }
        public int PhoneNumberTokenLength { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
