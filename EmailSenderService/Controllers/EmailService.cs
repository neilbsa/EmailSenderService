using EmailSenderService.Models;
using EmailSenderService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EmailSenderService.Controllers
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class RequestSizeLimitAttribute : Attribute, IAuthorizationFilter, IOrderedFilter
    {
        private readonly FormOptions _formOptions;

        public RequestSizeLimitAttribute(int valueCountLimit)
        {
            _formOptions = new FormOptions()
            {
                // tip: you can use different arguments to set each properties instead of single argument
                KeyLengthLimit = valueCountLimit,
                ValueCountLimit = valueCountLimit,
                ValueLengthLimit = valueCountLimit,

                // uncomment this line below if you want to set multipart body limit too
                MultipartBodyLengthLimit = valueCountLimit
            };
        }

        public int Order { get; set; }

        // taken from /a/38396065
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var contextFeatures = context.HttpContext.Features;
            var formFeature = contextFeatures.Get<IFormFeature>();

            if (formFeature == null || formFeature.Form == null)
            {
                // Setting length limit when the form request is not yet being read
                contextFeatures.Set<IFormFeature>(new FormFeature(context.HttpContext.Request, _formOptions));
            }
        }
    }









    [ApiController]
    [Route("api/v1/Services/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmailServiceController : ControllerBase
    {


        private readonly ISender sender;

        public EmailServiceController(ISender sender)
        {
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        [RequestSizeLimit(valueCountLimit: int.MaxValue)]
        public ActionResult SendEmail([FromForm] EmailPostRequest emailSendModel)
        {
            
            var forSend = new EmailMessage(emailSendModel);

            try
            {
                sender.SendEmail(forSend);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"Error: {e.Message.ToString()}");
          
            }
      
        }


        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<string>>> TestModule()
        {
            var testList = new List<string>() { "Test", "Was", "Success" };

            return await Task.FromResult(Ok(testList));
        }


    }
}
