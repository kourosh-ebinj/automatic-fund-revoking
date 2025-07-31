using System.Text.Json.Serialization;

namespace Application.Models.Requests.ThirdParties.Pasargad
{
    public class PasargadBankAuthenticationRq
    {
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

    }
}
