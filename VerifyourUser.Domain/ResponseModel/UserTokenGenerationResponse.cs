using System;
using System.Collections.Generic;
using System.Text;
using VerifyourUser.DataAccess.Entities;

namespace VerifyourUser.Domain.ResponseModel
{
    public class UserTokenGenerationResponse : UserCreationResponse
    {
        public UserVerificationTokenType UserVerificationTokenType { get; set; }
        public string Token { get; set; }
    }
}
