using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Abstractions
{
    public interface ISMSService
    {
        void  Send();

        Task SendAsync(string message, IEnumerable<string> recipientNumbers);

    }
}
