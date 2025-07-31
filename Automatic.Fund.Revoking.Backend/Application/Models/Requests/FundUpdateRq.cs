using System;

namespace Application.Models.Requests
{
    public record FundUpdateRq
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public int DsCode { get; set; }
        public bool IsEnabled { get; set; }

    }

}
