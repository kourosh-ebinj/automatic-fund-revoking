using System;

namespace Application.Models.Requests
{
    public record UserFundCreateRq
    {
        public long UserId { get; set; }
        public int FundId { get; set; }

    }

}
