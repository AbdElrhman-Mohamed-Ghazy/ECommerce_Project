using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Order.Queries.GetFullOrderInfoById
{
    public sealed record  GetFullOrderInfoByIdCommand(Guid OrderId):IRequest<FullOrderInfoDto>
    {
    }
}
