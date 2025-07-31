using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Extensions;
using Application.Models.Requests;
using Application.Models.Responses;
using Application.Services.Abstractions;
using Application.Services.Abstractions.Persistence;
using Application.Services.Abstractions.ThirdParties;
using Core.Constants;
using Core.Enums;
using Core.Models;
using Domain.Entities;
using Domain.Entities.ThirdParties;
using Domain.Extensions;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.EntityFramework
{

    public class CustomerService : CRUDService<Customer>, ICustomerService
    {
        public IRayanService _rayanService { get; set; }

        public CustomerService(IRayanService rayanService, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _rayanService = rayanService;

        }

        public IQueryable<Customer> GetQueryableByBackOfficeId(int fundId, IEnumerable<long> backOfficeIds, CancellationToken cancellationToken = default)
        {
            return GetAllQueryable(fundId, backOfficeIds: backOfficeIds.ToArray());

        }

        public async Task<CustomerRs> GetByBackOfficeId(int fundId, long backOfficeId, CancellationToken cancellationToken = default)
        {
            var customer = await GetAllQueryable(fundId, backOfficeIds: backOfficeId).FirstOrDefaultAsync();
            _guard.Assert(customer is not null, ExceptionCodeEnum.BadRequest, "شناسه رایان کاربر صحیج نیست.");

            return _mapper.Map<CustomerRs>(customer);
        }

        public async Task<CustomerRs> GetById(int fundId, long id, CancellationToken cancellationToken = default)
        {
            var customer = await GetByIdInternal(fundId, id);
            _guard.Assert(customer is not null, ExceptionCodeEnum.BadRequest, "شناسه کاربر صحیج نیست.");

            return _mapper.Map<CustomerRs>(customer);
        }

        public async Task<IEnumerable<CustomerRs>> GetByIds(int fundId, IEnumerable<long> ids, CancellationToken cancellationToken = default)
        {
            var customer = await GetByIdsInternal(fundId, ids, cancellationToken);
            _guard.Assert(customer is not null, ExceptionCodeEnum.BadRequest, "شناسه کاربر صحیج نیست.");

            return _mapper.Map<IEnumerable<CustomerRs>>(customer);
        }

        public async Task<PaginatedList<CustomerRs>> GetAll(int fundId, string keyword = "", int? pageSize = null, int? pageNumber = null, string orderBy = "",
                                                            CancellationToken cancellationToken = default)
        {
            var query = GetAllQueryable(fundId, keyword);
            var list = await query.ToPaginatedList(pageSize ?? GlobalConstants.DefaultPageSize, pageNumber ?? 1, orderBy, cancellationToken);


            var customers = _mapper.Map<ICollection<CustomerRs>>(list.Items);

            return new PaginatedList<CustomerRs>()
            {
                PageNumber = list.PageNumber,
                Items = customers,
                PageSize = list.PageSize,
                TotalItems = list.TotalItems,
                TotalPages = list.TotalPages,
            };
        }

        public IQueryable<Customer> GetAllQueryable(int fundId, string keyword = "", params long[] backOfficeIds)
        {
            var keywords = keyword.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var query = GetQuery()
                            .Include(e => e.Fund)
                            .AsQueryable();

            query = query.Where(e => e.FundId == fundId);

            if (backOfficeIds.Any())
                query = query.Where(e => backOfficeIds.Contains(e.BackOfficeId));

            foreach (var keywordItem in keywords)
            {
                query = query
                        .Where(x =>
                            x.FirstName.Contains(keywordItem) ||
                            x.LastName.Contains(keywordItem) ||
                            x.TradingCode.Contains(keywordItem) ||
                            x.NationalCode.Contains(keywordItem) ||
                            x.MobileNumber.Contains(keywordItem)
                        );
            }

            return query;
        }

        public async Task<CustomerRs> Create(int fundId, CustomerCreateRq request, CancellationToken cancellationToken = default)
        {
            await ValidateToCreate(fundId, request);

            var entity = _mapper.Map<Customer>(request);
            var fund = await base.Create(entity);
            await _unitOfWork.SaveChanges(cancellationToken);

            return _mapper.Map<CustomerRs>(fund);
        }

        public async Task<CustomerRs> Update(int fundId, CustomerUpdateRq request, CancellationToken cancellationToken = default)
        {
            var customer = await GetByIdInternal(request.FundId, request.Id);
            _guard.Assert(customer is not null, ExceptionCodeEnum.BadRequest, "مشتری یافت نشد.");

            await ValidateToUpdate(fundId, request);

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.NationalCode = request.NationalCode;
            customer.TradingCode = request.TradingCode;
            customer.MobileNumber = request.MobileNumber;

            await base.Update(customer);
            await _unitOfWork.SaveChanges(cancellationToken);

            return _mapper.Map<CustomerRs>(customer);
        }

        public async Task SyncAllCustomers(FundRs fund, CancellationToken cancellationToken = default)
        {
            var pageSize = 5000;
            int pageNumber = 0;
            int totalRecords = 0;

            do
            {
                pageNumber++;

                var paginatedRayanCustomers = await _rayanService.SyncRayanCustomers(fund, pageNumber: pageNumber, pageSize: pageSize,
                                                                                     cancellationToken: cancellationToken);
                if (totalRecords == 0)
                    totalRecords = paginatedRayanCustomers.TotalItems;

                await SyncCustomers(fund.Id, paginatedRayanCustomers.Result);

            }
            while ((pageSize * pageNumber) <= totalRecords);

            GC.Collect();
        }

        public async Task SyncCustomers(int fundId, IEnumerable<RayanCustomer> rayanCustomers, CancellationToken cancellationToken = default)
        {
            var rayanCustomerIds = rayanCustomers.Select(e => e.CustomerId);

            Func<IEnumerable<long>, Task<IEnumerable<Customer>>> tasks = async (IEnumerable<long> keys) =>
                await Process(fundId, keys, rayanCustomers, cancellationToken);

            _ = await tasks.DoItSplitted(rayanCustomerIds);
        }

        async Task<IEnumerable<Customer>> Process(int fundId, IEnumerable<long> rayanCustomerIds, IEnumerable<RayanCustomer> rayanCustomers, CancellationToken cancellationToken = default)
        {

            var customersInDbDic = await GetAllQueryable(fundId).Where(e => rayanCustomerIds.Contains(e.BackOfficeId)).ToDictionaryAsync(e => e.BackOfficeId, e => e);
            var newCustomerIds = rayanCustomerIds.Except(customersInDbDic.Keys);
            var newRayanCustomers = new List<RayanCustomer>();
            var newCustomers = new List<Customer>();

            if (newCustomerIds.Any())
            {
                newRayanCustomers = rayanCustomers.Where(e => newCustomerIds.Contains(e.CustomerId)).ToList();
                newCustomers = _mapper.Map<List<Customer>>(newRayanCustomers);
            }
            var existingCustomers = _mapper.Map<ICollection<Customer>>(rayanCustomers.Where(e => customersInDbDic.Keys.Contains(e.CustomerId)));

            newCustomers.AsParallel().ForAll(e => { e.Id = 0; e.FundId = fundId; });
            existingCustomers.AsParallel().ForAll(e =>
            {
                Customer entityInDb = null;
                if (customersInDbDic.ContainsKey(e.BackOfficeId))
                    entityInDb = customersInDbDic[e.BackOfficeId];

                e.Id = entityInDb.Id;
                e.FundId = fundId;
            });

            await Create(newCustomers);
            await Update(existingCustomers);
            await _unitOfWork.SaveChanges(cancellationToken);

            return newCustomers.Union(existingCustomers);
        }

        private async Task<Customer> GetByIdInternal(int fundId, long id, CancellationToken cancellationToken = default)
        {
            var customers = await GetByIdsInternal(fundId, new List<long>() { id }, cancellationToken);

            return customers.FirstOrDefault();
        }

        private async Task<IEnumerable<Customer>> GetByIdsInternal(int fundId, IEnumerable<long> ids, CancellationToken cancellationToken = default)
        {
            var customers = await GetAllQueryable(fundId)
                    .Where(e => ids.Contains(e.Id))
                    .ToListAsync(cancellationToken);

            return customers;
        }

        private async ValueTask ValidateToCreate(int fundId, CustomerCreateRq request)
        {
            await ValidateBase(fundId, request.FirstName, request.LastName, request.NationalCode, request.TradingCode, request.MobileNumber);

        }

        private async ValueTask ValidateToUpdate(int fundId, CustomerUpdateRq request)
        {
            await ValidateBase(fundId, request.FirstName, request.LastName, request.NationalCode, request.TradingCode, request.MobileNumber);

        }

        private async ValueTask ValidateBase(int fundId, string firstname, string lastname, string nationalCode, string tradingCode, string mobileNumber)
        {
            _guard.Assert(!string.IsNullOrWhiteSpace(firstname), ExceptionCodeEnum.BadRequest, "نام ضروری است");
            _guard.Assert(!string.IsNullOrWhiteSpace(lastname), ExceptionCodeEnum.BadRequest, "نام خانوادگی ضروری است");
            _guard.Assert(!string.IsNullOrWhiteSpace(nationalCode), ExceptionCodeEnum.BadRequest, "کد ملی ضروری است");
            _guard.Assert(!string.IsNullOrWhiteSpace(tradingCode), ExceptionCodeEnum.BadRequest, "کد بورسی ضروری است");
            _guard.Assert(!string.IsNullOrWhiteSpace(mobileNumber), ExceptionCodeEnum.BadRequest, "شماره موبایل ضروری است");

            var existingNationalCode = await GetAllQueryable(fundId).Where(e => e.NationalCode == nationalCode).FirstOrDefaultAsync();
            _guard.Assert(existingNationalCode is null, ExceptionCodeEnum.BadRequest, "کد ملی تکراری است.");

            var existingTradingCode = await GetAllQueryable(fundId).Where(e => e.TradingCode == tradingCode).FirstOrDefaultAsync();
            _guard.Assert(existingTradingCode is null, ExceptionCodeEnum.BadRequest, "کد بورسی تکراری است.");

            var existingMobileNo = await GetAllQueryable(fundId).Where(e => e.MobileNumber == mobileNumber).FirstOrDefaultAsync();
            _guard.Assert(existingMobileNo is null, ExceptionCodeEnum.BadRequest, "شماره موبایل تکراری است.");

            await Task.CompletedTask;
        }

    }
}
