using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Models;

namespace ECommerce.Api.Orders.Profiles
{
    public class OrderProfile:AutoMapper.Profile
    {
        public OrderProfile()
        {
            CreateMap<Db.Order,Models.Order>();
            CreateMap<Db.OrderItem,Models.OrderItem>();
        }
    }
}