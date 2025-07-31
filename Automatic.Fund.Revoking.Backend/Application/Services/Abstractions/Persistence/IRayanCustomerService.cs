using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities.ThirdParties;

namespace Application.Services.Abstractions.Persistence
{
    public interface IRayanCustomerService: ICRUDService<RayanCustomer>
    {
        Task<IEnumerable<RayanCustomer>> GetAll(int fundId);
        IQueryable<RayanCustomer> GetAllQueryable(int fundId);

        Task<IEnumerable<RayanCustomer>> CreateBatch(IEnumerable<RayanCustomer> request, CancellationToken cancellationToken = default);

    }
}
