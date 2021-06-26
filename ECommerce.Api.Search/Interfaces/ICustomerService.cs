using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Interfaces
{
    public interface ICustomerService
    {
        Task<(bool IsSuccess, dynamic Customer, string ErrorMessage )> GetCustomersAsync(int customerId);
    }
}