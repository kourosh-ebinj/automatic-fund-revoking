using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class SearchModel
    {
        public int id { get; set; }

        [Description("This is description on authorName property")]
        public string authorName { get; set; } = "";

    }
}
