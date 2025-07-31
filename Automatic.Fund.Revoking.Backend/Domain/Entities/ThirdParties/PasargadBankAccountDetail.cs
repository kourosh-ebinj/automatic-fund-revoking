using System;
using Domain.Abstractions;

namespace Domain.Entities.ThirdParties
{
    public class PasargadBankAccountDetail : EntityBase, ITrackable
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public string ScApiKeyAccountBalance { get; set; }
        public string ScApiKeyInternal { get; set; }
        public string ScApiKeyPaya { get; set; }
        public string ScApiKeySatna { get; set; }
        public string ScApiKeyKYC { get; set; }

        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedById { get; set; }

        public virtual BankAccount BankAccount{ get; set; }

    }
}
