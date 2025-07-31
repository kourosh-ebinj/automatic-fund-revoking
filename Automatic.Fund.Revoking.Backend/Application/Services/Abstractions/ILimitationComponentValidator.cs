using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Enums;
using Application.Models.Responses;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.Abstractions
{
    public interface ILimitationComponentValidator
    {
        LimitationComponentTypeEnum Type { get; }
        object GetValueAsObject(string jsonValue);
        
        Task<LimitationComponentValidatorResultRs> Validate(Limitation limitation, LimitationComponent limitationComponent, int fundId, Order order, CancellationToken cancellationToken = default);
        
        //Task<LimitationComponent> Generate(LimitationComponent limitationComponent);
    }
}
