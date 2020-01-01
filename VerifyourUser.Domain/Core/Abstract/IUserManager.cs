using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VerifyourUser.DataAccess.Entities;
using VerifyourUser.Domain.ResponseModel;

namespace VerifyourUser.Domain.Core.Abstract
{
    public interface IUserManager
    {
        Task<GenericResponseModel<UserCreationResponse>> CreateUser(string audienceId, string firstname, string lastname, string phonenumber, string email);
        Task<GenericResponseModel<UserVerificationResponse>> VerifyUser(string userId, string token, UserVerificationTokenType userVerificationTokenType);
        Task<GenericResponseModel<UserTokenGenerationResponse>> GenerateUserToken(string userId, string token, UserVerificationTokenType userVerificationTokenType);
        Task<GenericResponseModel<UserTokenGenerationResponse>> GetToken(string userId, UserVerificationTokenType userVerificationTokenType);
    }
}
