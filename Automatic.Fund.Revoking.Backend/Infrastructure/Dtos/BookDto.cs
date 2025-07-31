using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Dtos
{
    public class BookDto
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string authorName { get; set; }
        public string ISBN { get; set; }

    }
}
