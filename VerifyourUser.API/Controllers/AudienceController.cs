using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;
using VerifyourUser.API.BindingModels;
using VerifyourUser.Domain.Core.Abstract;
using VerifyourUser.Domain.ResponseModel;

namespace VerifyourUser.API.Controllers
{
    public class AudienceController : Controller
    {
        private IAudience _audienceService;
        private ILogger<AudienceController> _logger;
        private IHostingEnvironment _env;

        public AudienceController(IAudience audienceService, ILogger<AudienceController> logger, IHostingEnvironment env)
        {
            _audienceService = audienceService;
            _logger = logger;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AudienceBindingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var audienceExist = await _audienceService.GetAudienceByName(model.AppName);
                    if (audienceExist)
                    {
                        return Ok(GenerateResponse(null, false, "Application name exist"));
                    }

                    var audience = await _audienceService.SetupAudience(model.AppName, model.DisplayName);
                    if (audience != null)
                    {
                        audience.ClientSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(audience.ClientSecret));

                        return Ok(GenerateResponse(new
                        {
                            audience.ClientId,
                            audience.ClientSecret,
                            audience.AppName
                        }, true, "Application created successfully, the details is used to access the api resources."));
                    }
                }
                return Ok(GenerateResponse(ModelState, false, "All parameters is required"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                if (_env.IsDevelopment())
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    return BadRequest("An error occured.");
                }
            }
        }

        [Route("updateaudience"), HttpPost]
        public async Task<IActionResult> UpdateAudience([FromBody] UpdateAudienceBindingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var audience = await _audienceService.UpdateAudience(model.ClientId, model.Resources);
                    if (audience != null)
                    {
                        return Ok(new
                        {
                            model.ClientId,
                            model.Resources
                        });
                    }
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                if (_env.IsDevelopment())
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    return BadRequest("An error occured.");
                }
            }
        }

        #region Helpers
        private GenericResponseModel<object> GenerateResponse(object model, bool isSuccessful, string message)
        {
            return new GenericResponseModel<object>()
            {
                ResponseCode = isSuccessful ? "00" : "04",
                ResponseMessage = message,
                Data = model
            };
        }
        #endregion
    }
}