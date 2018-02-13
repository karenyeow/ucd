using Microsoft.AspNetCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Comlib.Common.Helpers.Email
{
    public interface IOneGovEmailSender
    {
        Task<bool> SendErrorEmailAsync(IExceptionHandlerFeature exception);
        Task<bool> SendEmailAsync(string message, string emailTo, string subject);
        bool SendErrorEmailSync(Exception exception);
        Task<bool> SendErrorEmailAsync(Exception exception);
    }
}
