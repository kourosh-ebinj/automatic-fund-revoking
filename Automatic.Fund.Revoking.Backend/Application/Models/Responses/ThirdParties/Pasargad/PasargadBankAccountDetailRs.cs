
namespace Application.Models.Responses.ThirdParties.Pasargad
{

    public record PasargadBankAccountDetailRs
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public string ScApiKeyAccountBalance { get; set; }
        public string ScApiKeyInternal { get; set; }
        public string ScApiKeyPaya { get; set; }
        public string ScApiKeySatna { get; set; }
        public string ScApiKeyKYC { get; set; }

    }
}
