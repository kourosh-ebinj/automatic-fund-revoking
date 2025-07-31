using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Application.Models.Responses;
using Application.Models.Responses.Reports;
using Core.Extensions;

namespace Application.Extensions
{
    public static class ReportExtensions
    {
        private static CultureInfo _faCulture => new CultureInfo("fa-IR");
        private static string FormatAsMoney(double amount) => amount.ToString("N", _faCulture);

        public static IEnumerable<OrderReportExcelRs> ToOrderReportExcelRs(this IEnumerable<OrderReportRs> entities)
        {
            var index = 1;

            return entities.Select(order => new OrderReportExcelRs()
            {
                Index = index++,
                AppName = order.AppName,
                BackOfficeOrderId = order.RayanFundOrderId.ToString(),
                CustomerAccountBankName = order.CustomerAccountBankName,
                CustomerAccountNumber = order.CustomerAccountNumber,
                CustomerAccountSheba = order.CustomerAccountSheba,
                CustomerFullName = order.CustomerFullName,
                CustomerId = order.CustomerId.ToString(),
                CustomerNationalCode = order.CustomerNationalCode,
                FundName = order.FundName,
                OrderStatus = order.OrderStatusId.ToDescription(),
                OrderStatusDescription = order.OrderStatusDescription,
                Title = order.Title,
                TotalAmount = FormatAsMoney(order.TotalAmount),
                TotalUnits = order.TotalUnits

            });
        }

        public static IEnumerable<BankPaymentReportExcelRs> ToBankPaymentReportExcelRs(this IEnumerable<BankPaymentReportRs> entities)
        {
            var index = 1;

            return entities.Select(transaction => new BankPaymentReportExcelRs()
            {
                Index = index++,
                BankPaymentMethod = transaction.BankPaymentMethodId.ToDescription(),
                BankUniqueId = transaction.BankUniqueId,
                Description = transaction.Description,
                DestinationBankName = transaction.DestinationBankName,
                DestinationShebaNumber = transaction.DestinationShebaNumber,
                OrderId = transaction.OrderId,
                SourceBankAccountNumber = transaction.SourceBankAccountNumber,
                SourceBankName = transaction.SourceBankName,
                TotalAmount = FormatAsMoney(transaction.TotalAmount),
                TransactionStatus = transaction.TransactionStatusId.ToDescription(),

            });
        }

        public static IEnumerable<OrderHistoryReportExcelRs> ToOrderHistoryReportRs(this IEnumerable<OrderHistoryReportRs> entities)
        {
            var index = 1;

            return entities.Select(orderHistory => new OrderHistoryReportExcelRs()
            {
                Index = index++,
                AppName = orderHistory.AppName,
                RayanFundOrderId = orderHistory.BackOfficeOrderId.ToString(),
                CustomerAccountBankName = orderHistory.CustomerAccountBankName,
                CustomerAccountNumber = orderHistory.CustomerAccountNumber,
                CustomerAccountSheba = orderHistory.CustomerAccountSheba,
                CustomerFullName = orderHistory.CustomerFullName,
                CustomerId = orderHistory.CustomerId.ToString(),
                CustomerNationalCode = orderHistory.CustomerNationalCode,
                FundName = orderHistory.FundName,
                OrderStatus = orderHistory.OrderStatusId.ToDescription(),
                OrderStatusDescription = orderHistory.OrderStatusDescription,
                Title = orderHistory.Title,
                TotalAmount = FormatAsMoney(orderHistory.TotalAmount),
                TotalUnits = orderHistory.TotalUnits

            });
        }
    }
}
