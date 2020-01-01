using System.Collections.Generic;
using System.Threading.Tasks;
using VerifyourUser.DataAccess.Entities;

namespace VerifyourUser.Domain.Core.Abstract
{
    public interface IAudience
    {
        Task<Audience> SetupAudience(string appName, string displayName);
        Task<Audience> UpdateAudience(string clientId, List<string> resources, List<string> roles = null, int emailTokenLength = 0, int phoneNumberTokenLength = 0);
        Task<Audience> GetAudience(string clientId);
        Task<Audience> GetAudienceByIdSecret(string clientId, string clientSecret);
        Task<bool> GetAudienceByName(string name);
    }
}
