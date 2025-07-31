using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models.Requests.ThirdParties.Rayan
{
    public class RayanFundCancellationRq
    {
        public string DsCode { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int orderStatusId { get; set; }

    }
}
