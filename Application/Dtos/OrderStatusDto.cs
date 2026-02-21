using Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class UpdateOrderStatusDto
    {
        public OrderStatus Status { get; set; }
    }
}
