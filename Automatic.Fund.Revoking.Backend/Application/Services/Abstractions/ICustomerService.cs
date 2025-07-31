using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Requests;
using Application.Models.Responses;
using Core.Models;
using Domain.Entities;
using Domain.Entities.ThirdParties;

namespace Application.Services.Abstractions
{
    public interface ICustomerService : ICRUDService<Customer>
    {
        IQueryable<Customer> GetQueryableByBackOfficeId(int fundId, IEnumerable<long> backOfficeIds, CancellationToken cancellationToken = default);
        Task<CustomerRs> GetByBackOfficeId(int fundId, long backOfficeId, CancellationToken cancellationToken = default);
        Task<CustomerRs> GetById(int fundId, long id, CancellationToken cancellationToken = default);
        Task<IEnumerable<CustomerRs>> GetByIds(int fundId, IEnumerable<long> ids, CancellationToken cancellationToken = default);
        Task<PaginatedList<CustomerRs>> GetAll(int fundId, string keyword = "", int? pageSize = null, int? pageNumber = null, string orderBy = "", CancellationToken cancellationToken = default);
        IQueryable<Customer> GetAllQueryable(int fundId, string keyword = "", params long[] backOfficeIds);
        Task<CustomerRs> Create(int fundId, CustomerCreateRq request, CancellationToken cancellationToken = default);
        Task<CustomerRs> Update(int fundId, CustomerUpdateRq request, CancellationToken cancellationToken = default);
        Task SyncAllCustomers(FundRs fund, CancellationToken cancellationToken = default);
        Task SyncCustomers(int fundId, IEnumerable<RayanCustomer> rayanCustomers, CancellationToken cancellationToken = default);

    }
}
