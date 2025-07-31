using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Abstractions
{
    public interface IEmailService
    {
        Task SendAsync(string content, IEnumerable<string> recipientEmails, bool IsHTML);

    }
}
