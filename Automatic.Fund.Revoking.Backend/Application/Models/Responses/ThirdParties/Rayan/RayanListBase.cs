using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Responses.ThirdParties.Rayan
{
    public record RayanListBase<T>
    {
        public RayanListBase()
        {

        }

        public IEnumerable<T> Result { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int Offset { get; set; }
        public int Total { get; set; }

    }
}
