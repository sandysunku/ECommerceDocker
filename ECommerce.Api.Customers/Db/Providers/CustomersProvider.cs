using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Api.Customers.Provider
{
    public class CustomersProvider: ICustomersProvider
    {
        private readonly CustomersDbContext customersDbContext;
        private readonly ILogger<CustomersDbContext> logger;
        private readonly IMapper mapper;

        public CustomersProvider(CustomersDbContext customersDbContext, ILogger<CustomersDbContext> logger, IMapper mapper)
        {
            this.customersDbContext = customersDbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if(!customersDbContext.Customers.Any())
            {
                customersDbContext.Customers.Add(new Customer{Id=1, Name="Charles", Address="No 20, Elix street, RM area"});
                customersDbContext.Customers.Add(new Customer{Id=2, Name="John", Address="No 200, Flix street, Gretin area"});
                customersDbContext.Customers.Add(new Customer{Id=3, Name="George", Address="No 50, Kex street, New York area"});
                customersDbContext.Customers.Add(new Customer{Id=4, Name="Kelvin", Address="No 10, Jelix street, Greater RM area"});
                customersDbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomerAsync(int id)
        {
            try
            {
                var customer = await customersDbContext.Customers.FirstOrDefaultAsync(x=> x.Id==id);
                if(customer!=null )
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(customer);
                    return (true, result,null);
                }

                return (false, null, "Not Found");
            }
            catch (System.Exception exception)
            {
                logger.LogError(exception.ToString());
                return (false, null, exception.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
             try
            {
                var customers = await customersDbContext.Customers.ToListAsync();
                if(customers!=null && customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(customers);
                    return (true, result,null);
                }

                return (false, null, "Not Found");
            }
            catch (System.Exception exception)
            {
                logger.LogError(exception.ToString());
                return (false, null, exception.Message);
            }
        }
    }
}