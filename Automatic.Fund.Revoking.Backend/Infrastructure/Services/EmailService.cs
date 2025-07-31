using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Abstractions;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendAsync(string content, IEnumerable<string> recipientEmails, bool IsHTML)
        {

            Console.WriteLine($"Email sent to {string.Join(", ", recipientEmails)}.");

            await Task.CompletedTask;
        }

    }
}
