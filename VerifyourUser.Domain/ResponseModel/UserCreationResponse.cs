using System;
using System.Collections.Generic;
using System.Text;

namespace VerifyourUser.Domain.ResponseModel
{
    public class UserCreationResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberConfirmed { get; set; }
        public string Email { get; set; }
        public string EmailConfirmed { get; set; }
    }
}
