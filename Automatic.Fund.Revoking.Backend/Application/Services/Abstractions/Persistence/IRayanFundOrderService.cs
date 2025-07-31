using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses.ThirdParties.Rayan;
using Domain.Entities.ThirdParties;

namespace Application.Services.Abstractions.Persistence
{
    public interface IRayanFundOrderService : ICRUDService<RayanFundOrder>
    {
        Task<IEnumerable<RayanFundOrder>> GetAll();
        Task<RayanFundOrder> GetById(long id);
        IQueryable<RayanFundOrder> GetAllQueryable();

        Task<RayanFundOrder> Create(RayanFundOrderRs request, CancellationToken cancellationToken = default);
        Task<IEnumerable<RayanFundOrder>> CreateBatch(IEnumerable<RayanFundOrderRs> request, CancellationToken cancellationToken = default);

    }
}
