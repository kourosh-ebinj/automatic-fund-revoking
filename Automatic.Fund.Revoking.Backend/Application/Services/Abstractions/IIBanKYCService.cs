using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Services.Abstractions
{
    public interface IIBanKYCService : ICRUDService<IBanKYC>
    {
        Task<IEnumerable<IBanKYC>> GetAll(CancellationToken cancellationToken = default);
        IQueryable<IBanKYC> GetAllQueryable();
        Task<IBanKYC> GetById(int id, CancellationToken cancellationToken = default);
        Task<IBanKYC> GetByIBan(string iban, CancellationToken cancellationToken = default);
        Task<IBanKYC> Create(string iBan, bool isKYCCompliant, CancellationToken cancellationToken = default);

    }
}
