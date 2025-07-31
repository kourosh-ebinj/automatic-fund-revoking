using AutoMapper;
using System;
using Domain.Entities;
using Application.Models.Responses;
using Application.Models.Requests;
using Domain.Entities.Audit;
using Core.Extensions;
using Domain.Entities.ThirdParties;
using Application.Models.Responses.ThirdParties.Rayan;
using Application.Models.Responses.ThirdParties.Pasargad;

namespace Presentation.Mappings.AutoMapper
{
    /// <summary>
    /// AutoMapper mappings must be added to this class
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<PasargadBankAccountDetail, PasargadBankAccountDetailRs>().ReverseMap();
            CreateMap<SagaTransaction, SagaTransactionCreateRq>().ReverseMap();
            CreateMap<SagaTransaction, SagaTransactionUpdateRq>().ReverseMap();
            CreateMap<SagaTransaction, SagaTransactionRs>().ReverseMap();

            CreateMap<Bank, BankRs>();

            CreateMap<Customer, CustomerRs>()
                .ForMember(dest => dest.FundName, conf => conf.MapFrom(src => src.Fund != null ? src.Fund.Name.Trim() : ""))

                .ReverseMap();

            CreateMap<RayanCustomerRs, RayanCustomer>()
                .ForMember(dest => dest.AccountNumber, conf => conf.MapFrom(src => src.AccountNumber.Trim()))
                .ForMember(dest => dest.BirthCertificationNumber, conf => conf.MapFrom(src => src.BirthCertificationNumber.Trim()))
                .ForMember(dest => dest.BirthDate, conf => conf.MapFrom(src => src.BirthDate.Trim()))
                .ForMember(dest => dest.BourseCode, conf => conf.MapFrom(src => src.BourseCode.Trim()))
                .ForMember(dest => dest.BranchName, conf => conf.MapFrom(src => src.BranchName.Trim()))
                .ForMember(dest => dest.CellPhone, conf => conf.MapFrom(src => src.CellPhone.Trim()))
                .ForMember(dest => dest.CreationDate, conf => conf.MapFrom(src => src.CreationDate.Trim()))
                .ForMember(dest => dest.CustomerFullName, conf => conf.MapFrom(src => src.CustomerFullName.Trim()))
                .ForMember(dest => dest.CustomerStatusName, conf => conf.MapFrom(src => src.CustomerStatusName.Trim()))
                .ForMember(dest => dest.FirstName, conf => conf.MapFrom(src => src.FirstName.Trim()))
                .ForMember(dest => dest.LastName, conf => conf.MapFrom(src => src.LastName.FixArabicAlphabet()))
                .ForMember(dest => dest.NationalIdentifier, conf => conf.MapFrom(src => src.NationalIdentifier.FixArabicNumeric()))
                .ForMember(dest => dest.Nationality, conf => conf.MapFrom(src => src.Nationality.Trim()))
                .ForMember(dest => dest.Personality, conf => conf.MapFrom(src => src.Personality.Trim()))
                .ForMember(dest => dest.Representative, conf => conf.MapFrom(src => src.Representative.Trim()))
                ;
            CreateMap<RayanCustomer, Customer>()
                .ForMember(dest => dest.FirstName, conf => conf.MapFrom(src => src.FirstName.FixArabicAlphabet()))
                .ForMember(dest => dest.LastName, conf => conf.MapFrom(src => src.LastName.FixArabicAlphabet()))
                .ForMember(dest => dest.MobileNumber, conf => conf.MapFrom(src => !string.IsNullOrWhiteSpace(src.CellPhone) ? src.CellPhone.SubstringToMaxLength(0, 11).FixArabicNumeric() : null))
                .ForMember(dest => dest.NationalCode, conf => conf.MapFrom(src => src.NationalIdentifier.Replace("-", "").FixArabicNumeric()))
                .ForMember(dest => dest.BackOfficeId, conf => conf.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.TradingCode, conf => conf.MapFrom(src => src.BourseCode))
                .ForMember(dest => dest.CreatedAt, conf => conf.Ignore())
                .ForMember(dest => dest.CreatedById, conf => conf.Ignore())
                .ForMember(dest => dest.ModifiedAt, conf => conf.Ignore())
                .ForMember(dest => dest.ModifiedById, conf => conf.Ignore())
                ;

            CreateMap<BankAccount, BankAccountRs>().ReverseMap();
            CreateMap<BankAccount, BankAccountCreateRq>().ReverseMap();
            CreateMap<BankAccount, BankAccountUpdateRq>().ReverseMap();

            CreateMap<UserFund, UserFundRs>();

            CreateMap<Fund, FundRs>().ReverseMap();
            CreateMap<Fund, FundCreateRq>().ReverseMap();
            CreateMap<Fund, FundUpdateRq>().ReverseMap();

            CreateMap<BankPayment, BankPaymentRs>()
                .ForMember(dest => dest.SourceBankAccountNumber, conf => conf.MapFrom(src => src.SourceBankAccount.AccountNumber))
                .ForMember(dest => dest.DestinationBankName, conf => conf.MapFrom(src => src.DestinationBank.Name))
                .ForMember(dest => dest.SourceBankName, conf => conf.MapFrom(src => src.SourceBankAccount.Bank.Name));

            CreateMap<RayanFundOrder, CancelledFundRs>().ReverseMap();
            CreateMap<RayanFundOrderRs, CancelledFundRs>().ReverseMap();
            CreateMap<RayanCustomerInfoRs, CustomerInfoRs>().ReverseMap();
            CreateMap<RayanBankAccountRs, CustomerBankAccountRs>().ReverseMap();

            CreateMap<RayanFundOrderRs, RayanFundOrder>().ReverseMap();

            CreateMap<OrderHistory, OrderHistoryRs>().ReverseMap();
            CreateMap<OrderUpdateRq, Order>().ReverseMap();
            CreateMap<OrderCreateRq, Order>().ReverseMap();
            CreateMap<Order, OrderHistory>()
                .ForMember(dest => dest.BackOfficeOrderId, conf => conf.MapFrom(src => src.RayanFundOrder.FundOrderId))
                .ForMember(dest => dest.OrderId, conf => conf.MapFrom(src => src.Id))
                .ForMember(dest => dest.ValidFrom, conf => conf.MapFrom((src, dest) =>
                {
                    DateTime result = DateTime.MinValue;
                    if (src.ModifiedAt is null)
                        result = src.CreatedAt;
                    else
                        result = src.ModifiedAt.Value;

                    return result;
                }))
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => 0));

            CreateMap<Order, OrderCreateRq>().ReverseMap();
            CreateMap<Order, OrderRs>()
                .ForMember(dest => dest.CustomerAccountBankName, conf => conf.MapFrom(src => src.CustomerAccountBank.Name))
                .ForMember(dest => dest.SagaTransactionId, conf => conf.MapFrom(src => src.SagaTransaction.Id))
                .ForMember(dest => dest.FundId, conf => conf.MapFrom(src => src.Customer.Fund.Id))
                .ForMember(dest => dest.FundName, conf => conf.MapFrom(src => src.Customer.Fund.Name));

            CreateMap<Limitation, LimitationRs>().ReverseMap();
            CreateMap<LimitationRs, AllLimitationsRs>().ReverseMap();
            CreateMap<Limitation, AllLimitationsRs>().ReverseMap();
            CreateMap<Limitation, LimitationRq>().ReverseMap();

            CreateMap<LimitationComponent, LimitationComponentRs>().ReverseMap();

        }
    }
}
