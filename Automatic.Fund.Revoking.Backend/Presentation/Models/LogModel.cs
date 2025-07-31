using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace Presentation.Models
{
    public class LogModel
    {
        [Required]
        public LogLevel LogType { get; set; }

        public string? Message { get; set; } 

        public string? CorrelationId { get; set; } 
        
        public string Details { get; set; }

        public ICollection<string> Parameters { get; set; }

    }
}
