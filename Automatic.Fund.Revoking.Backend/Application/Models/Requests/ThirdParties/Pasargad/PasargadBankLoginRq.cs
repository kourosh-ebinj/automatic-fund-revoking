using System.Text.Json.Serialization;

namespace Application.Models.Requests.ThirdParties.Pasargad
{
    public class PasargadBankLoginRq
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
