using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Services.Abstractions.Persistence;
using Domain.Entities.ThirdParties;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.EntityFramework
{

    public class RayanCustomerService : CRUDService<RayanCustomer>, IRayanCustomerService
    {
        public RayanCustomerService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<IEnumerable<RayanCustomer>> GetAll(int fundId)
        {
            var orderHistories = await GetAllQueryable(fundId).ToListAsync();

            return orderHistories;
        }

        public IQueryable<RayanCustomer> GetAllQueryable(int fundId)
        {
            var query = GetQuery().Where(e => e.FundId == fundId);

            return query;
        }

        public override async Task<RayanCustomer> Create(RayanCustomer entity, CancellationToken cancellationToken = default)
        {
            _guard.Assert(entity.Id == 0, Core.Enums.ExceptionCodeEnum.BadRequest);

            var rayanOrder = await base.Create(entity);

            return rayanOrder;
        }

        public override async Task<RayanCustomer> Update(RayanCustomer entity, CancellationToken cancellationToken = default)
        {
            _guard.Assert(entity.Id > 0, Core.Enums.ExceptionCodeEnum.BadRequest);

            var rayanOrder = await base.Update(entity);

            return rayanOrder;
        }

        public async Task<IEnumerable<RayanCustomer>> CreateBatch(IEnumerable<RayanCustomer> request, CancellationToken cancellationToken = default)
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
