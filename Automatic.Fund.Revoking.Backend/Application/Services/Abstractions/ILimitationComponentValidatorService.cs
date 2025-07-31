using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models.Responses;
using Domain.Entities;
using Domain.Enums;

namespace Application.Services.Abstractions
{
    public interface ILimitationComponentValidatorService
    {

        ILimitationComponentValidator GetLimitationComponentValidator(LimitationComponentTypeEnum limitationComponentType);

    }
}
