using Application.Dtos;
using AutoMapper;
using Domain.Entities.Orders;

namespace Application.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>();
        }
    }
}
