using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses.ThirdParties.Rayan;
using Application.Services.Abstractions.Persistence;
using Domain.Entities.ThirdParties;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.EntityFramework
{

    public class RayanFundOrderService : CRUDService<RayanFundOrder>, IRayanFundOrderService
    {
        public RayanFundOrderService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<IEnumerable<RayanFundOrder>> GetAll()
        {
            var rayanOrders = await GetAllQueryable().ToListAsync();

            return rayanOrders;
        }

        public async Task<RayanFundOrder> GetById(long id)
        {
            var rayanOrder = await GetAllQueryable().FirstOrDefaultAsync(e => e.Id == id);

            return rayanOrder;
        }

        public IQueryable<RayanFundOrder> GetAllQueryable()
        {
            var query = GetQuery();

            return query;
        }

        public async Task<RayanFundOrder> Create(RayanFundOrderRs request, CancellationToken cancellationToken = default)
        {

            var entity = _mapper.Map<RayanFundOrder>(request);
            var rayanOrder = await base.Create(entity);

            return rayanOrder;
        }


        public async Task<IEnumerable<RayanFundOrder>> CreateBatch(IEnumerable<RayanFundOrderRs> request, CancellationToken cancellationToken = default)
        {
            var ordersTasks = request.Select(async e =>
            {
                return await Create(e);
            });
            var orders = await Task.WhenAll(ordersTasks);
            await _unitOfWork.SaveChanges(cancellationToken);

            return orders;
        }

    }
}
