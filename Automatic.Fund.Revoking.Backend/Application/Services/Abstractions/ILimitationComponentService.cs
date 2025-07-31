using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Models.Requests;
using Application.Models.Responses;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.Abstractions
{
    public interface ILimitationComponentService : ICRUDService<LimitationComponent>
    {
        Task<IEnumerable<LimitationComponentRs>> GetAll(int fundId, bool? isEnabled = null, CancellationToken cancellationToken = default);
        Task<LimitationComponentRs> GetById(int limitationId, int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<LimitationComponentRs>> GetByLimitationTypeId(int fundId, LimitationTypeEnum limitationTypeId, CancellationToken cancellationToken = default);
        Task<IEnumerable<LimitationComponentRs>> GetByLimitationId(int fundId, int limitationId, CancellationToken cancellationToken = default);
        Task<object> GetLimitationComponentValue(int limitationId, int id, CancellationToken cancellationToken = default);
        IQueryable<LimitationComponent> GetAllQueryable(int fundId, bool? isEnabled = null);

        Task Update(LimitationComponentUpdateRq request, CancellationToken cancellationToken = default);
        
    }
}
