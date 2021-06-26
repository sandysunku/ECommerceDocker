using System.Linq;
using System.Threading.Tasks;
using ECommerce.Api.Search.Interfaces;

namespace ECommerce.Api.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrderService orderService;
        private readonly IProductService productsService;
        private readonly ICustomerService customerService;

        public SearchService(IOrderService orderService, IProductService productsService, ICustomerService customerService)
        {
            this.orderService = orderService;
            this.productsService = productsService;
            this.customerService = customerService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
           var orderResult = await orderService.GetOrdersAsync(customerId);
           var productsResult = await productsService.GetProductsAsync();
           var customerResult = await customerService.GetCustomersAsync(customerId);
           if(orderResult.IsSuccess)
           {
                foreach (var order in orderResult.Orders)
                {
                    foreach (var item in order.Items)
                    {
                        item.ProductName = productsResult.IsSuccess?
                            productsResult.Products.FirstOrDefault(x=>x.Id==item.ProductId)?.Name: "Product information is not available.";
                    }
                }
                

               var result = new{
                   Customer = customerResult.IsSuccess? customerResult.Customer: new {Name="Customer information is not available."},
                   Orders= orderResult.Orders
               };

               return(true, result);
           }

             return(false, null);
        }
    }
}