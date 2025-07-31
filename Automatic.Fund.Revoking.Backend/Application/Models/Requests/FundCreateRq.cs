using System;

namespace Application.Models.Requests
{
    public record FundCreateRq
    {
        public string Name { get; set; }
        public int DsCode { get; set; }
        public bool IsEnabled { get; set; }

    }

}
