using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses.ThirdParties.Pasargad;
using Application.Services.Abstractions.Caching;
using Application.Services.Abstractions.Persistence;
using Application.Services.Abstractions.ThirdParties.Banks;
using Core;
using Domain.Entities.ThirdParties;

namespace Infrastructure.Services.EntityFramework.ThirdParties.Banks
{
    public class PasargadBankAccountDetailService : CRUDService<PasargadBankAccountDetail>, IPasargadBankAccountDetailService
    {
        private IPasargadBankAccountDetailCacheService _pasargadBankAccountDetailCacheService => ServiceLocator.GetService<IPasargadBankAccountDetailCacheService>();

        public PasargadBankAccountDetailService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<PasargadBankAccountDetailRs> GetByBankAccountId(int bankAccountId, CancellationToken cancellationToken = default) =>
            await _pasargadBankAccountDetailCacheService.GetByBankAccountId(bankAccountId, cancellationToken);

        public async Task<IEnumerable<PasargadBankAccountDetailRs>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _pasargadBankAccountDetailCacheService.GetAll(cancellationToken);
            //var query = GetAllQueryable();
            //var items = await query.ToListAsync(cancellationToken);

            //return _mapper.Map<IEnumerable<PasargadBankAccountDetailRs>>(items);
        }

        public IQueryable<PasargadBankAccountDetail> GetAllQueryable()
        {
            var query = GetQuery();

            return query;
        }

    }
}
