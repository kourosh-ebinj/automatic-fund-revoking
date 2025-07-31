using System;

namespace Application.Models.Requests
{

    public record LimitationComponentUpdateRq
    {

        public int Id { get; set; }
        public string Value { get; set; }
        public bool Enabled { get; set; }
        public string Error { get; set; }
    }
}
