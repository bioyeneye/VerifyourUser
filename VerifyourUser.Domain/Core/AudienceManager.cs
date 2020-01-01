using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VerifyourUser.DataAccess.Context;
using VerifyourUser.DataAccess.Entities;
using VerifyourUser.Domain.Core.Abstract;

namespace VerifyourUser.Domain.Core
{
    public class AudienceManager : IAudience
    {
        private VerifyourUserDbContext _dbContext;
        private ILogger<AudienceManager> _logger;
        private OpenIddictApplicationManager<OpenIddictApplication> _openIddictConnectStore;
        private OpenIddictScopeManager<OpenIddictScope> _openIddictScopeManager;
        public AudienceManager(VerifyourUserDbContext dbContext, OpenIddictApplicationManager<OpenIddictApplication> openlddictConnectStore, ILogger<AudienceManager> logger, OpenIddictScopeManager<OpenIddictScope> openIddictScopeManager)
        {
            _dbContext = dbContext;
            _openIddictConnectStore = openlddictConnectStore;
            _logger = logger;
            _openIddictScopeManager = openIddictScopeManager;
        }

        public async Task<bool> GetAudienceByName(string name)
        {
            var AudiencesRepo = _dbContext.Audiences;
            return await AudiencesRepo.AnyAsync(c => c.AppName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Audience> GetAudience(string clientId)
        {
            var AudiencesRepo = _dbContext.Audiences;
            return await AudiencesRepo.FirstOrDefaultAsync(c => c.ClientId == clientId);
        }

        public async Task<Audience> GetAudienceByIdSecret(string clientId, string clientSecret)
        {
            var AudiencesRepo = _dbContext.Audiences;
            try
            {
                clientSecret = Encoding.UTF8.GetString(Convert.FromBase64String(clientSecret));
                return await AudiencesRepo.FirstOrDefaultAsync(c => c.ClientId == clientId && c.ClientSecret.Equals(clientSecret, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Audience> SetupAudience(string appName, string displayName)
        {
            //todo: implement name check
            string clientId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15).ToUpper();
            var AudiencesRepo = _dbContext.Audiences;
            string secret = Guid.NewGuid().ToString().ToUpper();
            string base64Secret = Convert.ToBase64String(Encoding.UTF8.GetBytes(secret));
            Audience Audience = new Audience();
            Audience.AppName = appName;
            Audience.ClientId = clientId;
            Audience.ClientSecret = secret;
            Audience.CreatedOn = DateTime.Now;
            await AudiencesRepo.AddAsync(Audience);
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = clientId,
                ClientSecret = base64Secret,
                DisplayName = displayName,
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    OpenIddictConstants.Permissions.GrantTypes.Password,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken
                }
            };

            await _openIddictConnectStore.CreateAsync(descriptor);
            await _dbContext.SaveChangesAsync();
            return Audience;
        }

        public async Task<Audience> UpdateAudience(string clientId, List<string> resources, List<string> roles, int emailTokenLength = 0, int phoneNumberTokenLength = 0)
        {
            var AudiencesRepo = _dbContext.Audiences;
            var Audiences = await AudiencesRepo.FirstOrDefaultAsync(c => c.ClientId == clientId);
            if (Audiences != null)
            {
                if (emailTokenLength > 0)
                {
                    Audiences.EmailTokenLength = emailTokenLength;
                }

                if (phoneNumberTokenLength > 0)
                {
                    Audiences.PhoneNumberTokenLength = phoneNumberTokenLength;
                }

                Audiences.Resources = string.Join(",", resources);
                if (roles?.Count > 0)
                {
                    Audiences.Channels = string.Join(",", roles);
                }
                await _dbContext.SaveChangesAsync();
            }
            return Audiences;
        }
    }
}
