using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using VerifyourUser.Domain.Core.Abstract;
using static AspNet.Security.OpenIdConnect.Primitives.OpenIdConnectConstants;
using Claims = AspNet.Security.OpenIdConnect.Primitives.OpenIdConnectConstants.Claims;

namespace VerifyourUser.API.Controllers
{
    public class AuthorizationController : Controller
    {
        private ILogger<AuthorizationController> _logger;
        private IAudience _audienceService;
        public AuthorizationController(ILogger<AuthorizationController> logger, IAudience audienceService)
        {
            _logger = logger;
            _audienceService = audienceService;
        }

        [AllowAnonymous]
        [HttpPost("~/oauth/token")]
        public async Task<IActionResult> Exchange(OpenIdConnectRequest request)
        {
            try
            {
                _logger.LogInformation("About to authorize user");
                if (request.IsClientCredentialsGrantType())
                {
                    var authenticationTicket = await CreateTicket(request);

                    //this occur when the client credentials are wrong
                    if (authenticationTicket == null)
                    {
                        var properties = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [Properties.Error] = Errors.InvalidGrant,
                            [Properties.ErrorDescription] = "The client id/secret couple is invalid."
                        });

                        return Forbid(properties, OpenIddictServerDefaults.AuthenticationScheme);
                    }
                    _logger.LogInformation("Client Credentials authentication successfully.");
                    return SignIn(authenticationTicket.Principal, authenticationTicket.Properties, authenticationTicket.AuthenticationScheme);
                }
                return BadRequest("Invalid grant type");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        #region Helpers 
        private async Task<AuthenticationTicket> CreateTicket(OpenIdConnectRequest request, AuthenticationProperties properties = null)
        {
            //check if the client credentials exist
            var audience = await _audienceService.GetAudienceByIdSecret(request.ClientId, request.ClientSecret);
            if (audience == null)
            {
                return null;
            }

            //var roles = audience.Channels?.Split(',');
            var principal = CreateClaims(request, audience.AppName);
            var ticket = new AuthenticationTicket(principal, properties, OpenIdConnectServerDefaults.AuthenticationScheme);
            string resources = audience.Resources;
            if (!request.IsRefreshTokenGrantType())
            {
                foreach (var claim in principal.Claims)
                {
                    switch (claim.Type)
                    {
                        case Claims.Email:
                            if (request.HasScope(OpenIdConnectConstants.Scopes.Email))
                                claim.SetDestinations(Destinations.IdentityToken);
                            break;
                        case Claims.PhoneNumber:
                            if (request.HasScope(OpenIdConnectConstants.Scopes.Phone))
                                claim.SetDestinations(Destinations.IdentityToken);
                            break;
                        case Claims.Picture:
                            if (request.HasScope(OpenIdConnectConstants.Scopes.Profile))
                                claim.SetDestinations(Destinations.IdentityToken);
                            break;
                        case Claims.Region:
                            break;
                        default:
                            claim.SetDestinations(Destinations.AccessToken,
                                                  Destinations.IdentityToken);
                            break;
                    }
                }

                ticket.SetScopes(new[]
                {
                    Scopes.OpenId,
                    Scopes.Email,
                    Scopes.Profile,
                    Scopes.OfflineAccess,
                    OpenIddictConstants.Scopes.Roles
                }.Intersect(request.GetScopes()));
            }
            ticket.SetResources(resources.Split(','));
            return ticket;
        }
        private ClaimsPrincipal CreateClaims(OpenIdConnectRequest request, string appname = "", string[] roles = null)
        {
            var identity = new ClaimsIdentity(OpenIdConnectServerDefaults.AuthenticationScheme, Claims.Name, Claims.Role);

            identity.AddClaim(Claims.Subject, request.ClientId, Destinations.AccessToken);
            identity.AddClaim(Claims.ClientId, request.ClientId, Destinations.AccessToken);
            identity.AddClaim(Claims.Name, appname);
            //if (roles?.Count() > 0)
            //{
            //    foreach (var role in roles)
            //    {
            //        identity.AddClaim(OpenIdConnectConstants.Claims.Role, role);
            //    }
            //}
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }
        #endregion
    }
}