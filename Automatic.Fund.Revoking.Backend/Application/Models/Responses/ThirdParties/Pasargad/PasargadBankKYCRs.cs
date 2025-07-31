using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.Models.Responses.ThirdParties.Pasargad
{

    public record PasargadBankKYCRs
    {
        public bool Matched { get; set; }

    }
}
