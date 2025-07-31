using System;
using System.Linq;
using Application.Models.Responses;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Models.Requests;
using System.Threading;
using Application.Services.Abstractions.Persistence;
using Core.Enums;
using Domain.Enums;
using System.Collections;
using System.Collections.Generic;
using Core.Models;

namespace Application.Services.Abstractions
{
    public interface ISagaTransactionService : ICRUDService<SagaTransaction>
    {
        Task<PaginatedList<SagaTransactionRs>> GetAll(int fundId, CancellationToken cancellationToken = default);
        Task<SagaTransactionRs> GetById(long id, CancellationToken cancellationToken = default);
        Task<SagaTransactionRs> GetByOrderIdOrDefault(long orderId, CancellationToken cancellationToken = default);
        Task<SagaTransaction> GetByOrderIdWithLock(long orderId, SagaTransactionStatusEnum? state = null, CancellationToken cancellationToken = default);

        Task Create(SagaTransactionCreateRq request, CancellationToken cancellationToken = default);
        Task Update(SagaTransaction entity, SagaTransactionStatusEnum currentStatus, CancellationToken cancellationToken = default);


    }
}
