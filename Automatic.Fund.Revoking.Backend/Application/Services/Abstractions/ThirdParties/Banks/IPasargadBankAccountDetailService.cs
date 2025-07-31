using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Responses.ThirdParties.Pasargad;
using Domain.Entities.ThirdParties;

namespace Application.Services.Abstractions.ThirdParties.Banks
{
    public interface IPasargadBankAccountDetailService
    {
        Task<IEnumerable<PasargadBankAccountDetailRs>> GetAll(CancellationToken cancellationToken = default);
        Task<PasargadBankAccountDetailRs> GetByBankAccountId(int bankAccountId, CancellationToken cancellationToken = default);
        IQueryable<PasargadBankAccountDetail> GetAllQueryable();

    }
}
