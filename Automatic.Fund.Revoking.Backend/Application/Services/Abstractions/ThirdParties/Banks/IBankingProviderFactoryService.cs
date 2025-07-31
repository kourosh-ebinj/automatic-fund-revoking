using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Requests;

namespace Application.Services.Abstractions.ThirdParties.Banks
{
    public interface IBankingProviderFactoryService
    {
        Task<IBankingProviderService> InstantiateBankingProvider(int fundId, int bankId, CancellationToken cancellationToken = default);


    }
}
