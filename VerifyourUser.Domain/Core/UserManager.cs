using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VerifyourUser.DataAccess.Context;
using VerifyourUser.DataAccess.Entities;
using VerifyourUser.Domain.Core.Abstract;
using VerifyourUser.Domain.ResponseModel;

namespace VerifyourUser.Domain.Core
{
    public class UserManager : IUserManager
    {
        private VerifyourUserDbContext _dbContext;
        private ILogger<UserManager> _logger;

        public UserManager(VerifyourUserDbContext dbContext, ILogger<UserManager> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task<GenericResponseModel<UserCreationResponse>> CreateUser(string audienceId, string firstname, string lastname, string phonenumber, string email)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponseModel<UserTokenGenerationResponse>> GenerateUserToken(string userId, string token, UserVerificationTokenType userVerificationTokenType)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponseModel<UserTokenGenerationResponse>> GetToken(string userId, UserVerificationTokenType userVerificationTokenType)
        {
            throw new NotImplementedException();
        }

        public Task<GenericResponseModel<UserVerificationResponse>> VerifyUser(string userId, string token, UserVerificationTokenType userVerificationTokenType)
        {
            throw new NotImplementedException();
        }
    }
}
