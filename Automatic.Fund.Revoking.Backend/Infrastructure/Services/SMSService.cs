using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Abstractions;

namespace Infrastructure.Services
{
    public class SMSService : ISMSService
    {
        public async Task SendAsync(string message, IEnumerable<string> recipientNumbers)
        {

            Console.WriteLine($"SMS sent to {string.Join(", ", recipientNumbers)}.");

            await Task.CompletedTask;
        }

        public void Send()
        {
            var recipientNumbers = new List<string>() { "09122942597" };
            Console.WriteLine($"SMS sent to {string.Join(", ", recipientNumbers)}.");
        }
    }
}
