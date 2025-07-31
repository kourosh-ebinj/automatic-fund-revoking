using System.Text.Json.Serialization;

namespace Application.Models.Responses.ThirdParties.Pasargad
{
    public record PasargadBankLoginRs
    {
        public string AccessToken { get; set; }

    }
}
