using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Responses;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Abstractions
{
    public interface ILimitationService : ICRUDService<Limitation>
    {
        Task<IEnumerable<AllLimitationsRs>> GetAll(int fundId);
        IQueryable<Limitation> GetAllQueryable(int fundId);
        Task<LimitationRs> GetLimitationByLimitationType(int fundId, LimitationTypeEnum limitationType);
        Task<LimitationRs> GetById(int fundId, int id);

        Task<LimitationComponentValidatorResultRs> Validate(LimitationTypeEnum limitationType, int fundId, Order order);

    }
}
