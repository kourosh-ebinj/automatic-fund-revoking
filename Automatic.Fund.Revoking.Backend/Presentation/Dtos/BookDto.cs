using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Dtos
{
    public class BookDto
    {
        public int id { get; set; }
        [Description("This is the title of the book")]
        public string Title { get; set; }
        public string authorName { get; set; }
        [Description("This is the shabak")]
        [RegularExpression(@"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$")]
        public string ISBN { get; set; }

    }
}
