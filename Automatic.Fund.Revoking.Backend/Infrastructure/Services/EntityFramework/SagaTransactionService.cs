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
using Core.Constants;
using Core.Enums;
using Core.Models;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.EntityFramework
{

    public class SagaTransactionService : CRUDService<SagaTransaction>, ISagaTransactionService
    {

        public SagaTransactionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<PaginatedList<SagaTransactionRs>> GetAll(int fundId, CancellationToken cancellationToken = default)
        {
            var sagaTransactions = await GetAllQueryable(fundId)
                .ToPaginatedList(GlobalConstants.DefaultPageSize, 1, "", cancellationToken);

            var items = _mapper.Map<ICollection<SagaTransactionRs>>(sagaTransactions.Items);

            return new PaginatedList<SagaTransactionRs>()
            {
                PageNumber = sagaTransactions.PageNumber,
                Items = items,
                PageSize = sagaTransactions.PageSize,
                TotalItems = sagaTransactions.TotalItems,
                TotalPages = sagaTransactions.TotalPages,
            };
        }

        public async Task<SagaTransactionRs> GetById(long id, CancellationToken cancellationToken = default)
        {
            var sagaTransaction = await GetByIdInternal(id, cancellationToken);
            _guard.Assert(sagaTransaction is not null, System.Net.HttpStatusCode.NotFound, "شناسه ساگا یافت نشد.");

            return _mapper.Map<SagaTransactionRs>(sagaTransaction);
        }

        public async Task<SagaTransactionRs> GetByOrderIdOrDefault(long orderId, CancellationToken cancellationToken = default)
        {
            var query = GetAllQueryable();
            query = query.Where(e => e.OrderId == orderId);

            var sagaTransaction = await query.FirstOrDefaultAsync(cancellationToken);
            if (sagaTransaction is null) return null;

            return _mapper.Map<SagaTransactionRs>(sagaTransaction);
        }

        public async Task<SagaTransaction> GetByOrderIdWithLock(long orderId, SagaTransactionStatusEnum? state = null, CancellationToken cancellationToken = default)
        {
            var query = FromSql("SELECT * FROM dbo.SagaTransactions WITH (ROWLOCK, XLOCK)");

            if (state is not null)
                query = query.Where(e => e.Status == state);

            return await query.FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);
        }

        public async Task Create(SagaTransactionCreateRq request, CancellationToken cancellationToken = default)
        {
            try
            {
                await ValidateToCreate(request);

                var entity = _mapper.Map<SagaTransaction>(request);
                _ = await base.Create(entity);
                await _unitOfWork.SaveChanges(cancellationToken);

            }
            catch (Exception ex)
            {

                throw;
            }

            return;
        }

        public async Task Update(SagaTransaction entity, SagaTransactionStatusEnum currentStatus, CancellationToken cancellationToken = default)
        {
            try
            {
                //var currentSagaTransaction = await GetByIdInternal(request.Id);
                //_guard.Assert(currentSagaTransaction is not null, ExceptionCodeEnum.BadRequest, "تراکنش ساگا با این شناسه یافت نشد.");

                await ValidateToUpdate(entity, currentStatus, entity.Status);

                await base.Update(entity);
                await _unitOfWork.SaveChanges(cancellationToken);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<SagaTransaction> GetByIdInternal(long id, CancellationToken cancellationToken = default) =>
             await Find(id, cancellationToken);

        private IQueryable<SagaTransaction> GetAllQueryable(int? fundId = null)
        {
            var query = GetQuery();

            if (fundId.HasValue)
                query = query
                    .Include(e => e.Order).ThenInclude(e => e.Customer)
                    .Where(e => e.Order.Customer.FundId == fundId);

            return query.OrderByDescending(e => e.OrderId).ThenByDescending(e => e.CreatedById);
        }

        private async ValueTask ValidateToCreate(SagaTransactionCreateRq request)
        {
            await ValidateBase(request.OrderId, request.Description);

        }

        private async ValueTask ValidateToUpdate(SagaTransaction request, SagaTransactionStatusEnum currentStatus, SagaTransactionStatusEnum newStatus)
        {
            await ValidateBase(request.OrderId, request.Description);

            switch (currentStatus)
            {
                case SagaTransactionStatusEnum.WaitingToGetPaid:
                    break;
                case SagaTransactionStatusEnum.WaitingToGetMarkedAsConfirmed:
                    _guard.Assert(newStatus == SagaTransactionStatusEnum.MarkedAsConfirmed, ExceptionCodeEnum.BadRequest, $"امکان تغییر وضعیت تراکنش ساگا فقط به MarkedAsConfirmed وجود دارد. OrderId: {request.OrderId}");
                    break;
                case SagaTransactionStatusEnum.WaitingToGetReversed:
                    _guard.Assert(newStatus == SagaTransactionStatusEnum.ReversedToDraft, ExceptionCodeEnum.BadRequest, $"امکان تغییر وضعیت تراکنش ساگا فقط به ReversedToDraft وجود دارد. OrderId: {request.OrderId}");
                    break;

                case SagaTransactionStatusEnum.MarkedAsConfirmed:
                    _guard.Assert(newStatus == SagaTransactionStatusEnum.Paid ||
                                  newStatus == SagaTransactionStatusEnum.WaitingToGetReversed,
                        ExceptionCodeEnum.BadRequest, $"امکان تغییر وضعیت تراکنش ساگا فقط به Paid | WaitingToGetReversed وجود دارد. OrderId: {request.OrderId}");
                    break;
                case SagaTransactionStatusEnum.Paid:
                    _guard.Assert(newStatus == SagaTransactionStatusEnum.Paid, ExceptionCodeEnum.BadRequest, $"امکان تغییر وضعیت تراکنش ساگا فقط به ReversedToDraft وجود دارد. OrderId: {request.OrderId}");
                    break;
                case SagaTransactionStatusEnum.ReversedToDraft:
                    _guard.Assert(false, ExceptionCodeEnum.BadRequest, "امکان تغییر وضعیت سفارش در وضعیت ReversedToDraft وجود ندارد.");
                    break;
                default:
                    throw new NotImplementedException("این وضعیت ساگا پیاده سازی نشده است.");
            }
        }

        private async ValueTask ValidateBase(long orderId, string description)
        {
            _guard.Assert(orderId > 0, ExceptionCodeEnum.BadRequest, "مقدار شناسه سفارش نا معتبر است.");

            await Task.CompletedTask;
        }
    }
}
