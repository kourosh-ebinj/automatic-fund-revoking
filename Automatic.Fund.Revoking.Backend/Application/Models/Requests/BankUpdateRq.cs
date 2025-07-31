using System;

namespace Application.Models.Requests
{
    public record BankUpdateRq
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public int BackOfficeBankId { get; set; }
        //public string ProviderClassName { get; set; }
        public bool IsEnabled { get; set; }

    }

}
