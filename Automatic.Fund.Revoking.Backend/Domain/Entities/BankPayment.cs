using System;
using System.Collections.Generic;
using Domain.Abstractions;
using Core.DomainValidation.Helpers;
using Domain.Enums;

namespace Domain.Entities
{
    public class BankPayment : EntityBase, IValidate, ITrackable
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public int SourceBankAccountId { get; set; }
        public string DestinationShebaNumber { get; set; }
        public int DestinationBankId { get; set; }
        public double TotalAmount { get; set; }
        public string BankUniqueId { get; set; }
        public string Description { get; set; }
        public BankPaymentMethodEnum BankPaymentMethodId { get; set; }
        public TransactionStatusEnum TransactionStatusId { get; set; }

        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedById { get; set; }

        public Bank DestinationBank { get; set; }
        public BankAccount SourceBankAccount { get; set; }
        public Order Order { get; set; }
        public void Validate()
        {

            #region TotalAmount

            if (TotalAmount < 1)
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(TotalAmount)));
            #endregion

            #region OrderId

            if (OrderId < 1)
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(OrderId)));

            #endregion


        }

    }
}
