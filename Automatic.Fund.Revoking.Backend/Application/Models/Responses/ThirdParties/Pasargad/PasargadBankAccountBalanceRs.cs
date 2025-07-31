using System.Text.Json.Serialization;

namespace Application.Models.Responses.ThirdParties.Pasargad
{

    public record PasargadBankAccountBalanceRs
    {
        public string DepositNumber { get; set; }
        public double DepositBalance { get; set; }
        public double DepositAvailableBalance { get; set; }
        public string UID { get; set; }
    }
}
