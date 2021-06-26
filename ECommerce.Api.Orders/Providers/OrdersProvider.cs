using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using ECommerce.Api.Orders.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System;

namespace ECommerce.Api.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext ordersDbContext;
        private readonly ILogger<OrdersDbContext> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext ordersDbContext, ILogger<OrdersDbContext> logger, IMapper mapper)
        {
            this.ordersDbContext = ordersDbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if(!ordersDbContext.Orders.Any())
            {
                ordersDbContext.Orders.Add(new Db.Order{Id=1, CustomerId=1, OrderDate=DateTime.Now, Total=200, 
                Items=new List<Db.OrderItem>{ new Db.OrderItem{Id=1, ProductId=1,Quantity=20,UnitPrice=400}}});
                ordersDbContext.Orders.Add(new Db.Order{Id=2, CustomerId=2, OrderDate=DateTime.Now, Total=300, 
                Items=new List<Db.OrderItem>{ new Db.OrderItem{Id=2, ProductId=2,Quantity=250,UnitPrice=450}}});
                ordersDbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await ordersDbContext.Orders
                    .Where(o => o.CustomerId == customerId)
                    .Include(o => o.Items)
                    .ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, 
                        IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }  
        }
    }
}