using System;
using System.Threading.Tasks;
using Comlib.Common.Helpers.Email;
using Microsoft.AspNetCore.Mvc;

namespace iCare.Api.Controllers
{
    public class AbstractApiController : Controller
    {
        private readonly IOneGovEmailSender _oneGovEmailSender;

        public AbstractApiController(IOneGovEmailSender oneGovEmailSender)
        {
            _oneGovEmailSender = oneGovEmailSender;
        }

        protected async Task<IActionResult> HandleException(Exception ex)
        {
            await _oneGovEmailSender.SendErrorEmailAsync(ex);
            return new BadRequestObjectResult(new
            {
                Error = ex.HResult.ToString(),
                ErrorDescription = ex.Message
            });
        }
    }
}
