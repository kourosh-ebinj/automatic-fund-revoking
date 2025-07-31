using System;

namespace Application.Models.Requests
{
    public record CustomerCreateRq
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string TradingCode { get; set; }
        public string MobileNumber { get; set; }

    }

}
