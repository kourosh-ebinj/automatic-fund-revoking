using System;
using System.Collections.Generic;
using Domain.Abstractions;
using Core.DomainValidation.Helpers;
using Domain.Enums;
using Domain.Entities.ThirdParties;
using Domain.Entities.Audit;

namespace Domain.Entities
{
    public class Order : EntityBase, IValidate, IsSoftDeletable, ITrackable, IAuditable
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public double TotalAmount { get; set; }
        public int TotalUnits { get; set; }
        public OrderStatusEnum OrderStatusId { get; set; }
        public string OrderStatusDescription { get; set; }

        /// <summary>
        /// کد سامانه درخواست کننده
        /// </summary>
        public string AppCode { get; set; }

        /// <summary>
        /// نام سامانه درخواست کننده
        /// </summary>
        public string AppName { get; set; }
        public long CustomerId { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerNationalCode { get; set; }
        public string CustomerAccountSheba { get; set; }
        public string CustomerAccountNumber { get; set; }
        public int CustomerAccountBankId { get; set; }
        //public int FundId { get; set; }
        //public long? BankPaymentId { get; set; }
        public long RayanFundOrderId { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public long CreatedById { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public long? ModifiedById { get; set; }

        public RayanFundOrder RayanFundOrder { get; set; }
        public Bank CustomerAccountBank { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual SagaTransaction SagaTransaction { get; set; }
        public virtual ICollection<OrderHistory> OrderHistories { get; set; }

        public void Validate()
        {
            #region Title

            if (string.IsNullOrWhiteSpace(Title))
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(Title)));

            if (Title.Length < 5)
                _ValidationErrors.Add(ErrorsHelper.MinLengthError(nameof(Title)));
            #endregion

            #region TotalAmount

            if (TotalAmount < 1)
                _ValidationErrors.Add(ErrorsHelper.EmptyError(nameof(TotalAmount)));
            #endregion

        }
    }
}
