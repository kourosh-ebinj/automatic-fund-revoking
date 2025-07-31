using System;

namespace Application.Models.Requests
{
    public record UserFundUpdateRq
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public int fundId { get; set; }

    }

}
