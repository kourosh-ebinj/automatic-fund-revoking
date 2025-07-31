using System;
using System.Collections.Generic;
using Domain.Entities.ThirdParties;

namespace Application.Models.Responses.ThirdParties.Rayan
{
    public record PaginatedRayanCustomerRs
    {
        public IEnumerable<RayanCustomer> Result { get; set; }

        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalItems { get; set; }

    }

}
