using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Requests;
using Application.Models.Responses;
using Domain.Entities;

namespace Application.Services.Abstractions
{
    public interface IUserFundService : ICRUDService<UserFund>
    {
        Task<IEnumerable<FundRs>> GetUserFunds(long userId, IEnumerable<string> userRoles, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserFundRs>> GetAll(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserFundRs>> GetByUserId(long userId ,CancellationToken cancellationToken = default);
        IQueryable<UserFund> GetAllQueryable(long? userId = null);

        Task<UserFundRs> Create(UserFundCreateRq request, CancellationToken cancellationToken = default);
        Task<UserFundRs> Update(UserFundUpdateRq request, CancellationToken cancellationToken = default);
        Task Delete(int id, CancellationToken cancellationToken = default);
    }
}
