using ECommerce.Api.Products.Providers;
using System;
using System.Linq;
using AutoMapper;
using ECommerce.Api.Products.Db;
using Xunit;
using Microsoft.EntityFrameworkCore;
using ECommerce.Api.Products.Profiles;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Tests
{
    public class ProductsServiceTest
    {
        [Fact]
        public async Task GetProductReturnsAllProducts()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductReturnsAllProducts)).Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            var profile = new ProductProfile();
            var configuration = new MapperConfiguration(config => config.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var products = await productsProvider.GetProductsAsync();

            Assert.True(products.IsSuccess);
            Assert.True(products.products.Any());
            Assert.Null(products.ErrorMessage);
        }

        [Fact]
        public async Task GetProductReturnProductUsingValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductReturnProductUsingValidId)).Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            var profile = new ProductProfile();
            var configuration = new MapperConfiguration(config => config.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var products = await productsProvider.GetProductAsync(1);

            Assert.True(products.IsSuccess);
            Assert.NotNull(products.Product);
            Assert.Null(products.ErrorMessage);
        }

        [Fact]
        public async Task GetProductReturnProductUsingInValidId()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(nameof(GetProductReturnProductUsingInValidId)).Options;
            var dbContext = new ProductsDbContext(options);
            CreateProducts(dbContext);

            var profile = new ProductProfile();
            var configuration = new MapperConfiguration(config => config.AddProfile(profile));
            var mapper = new Mapper(configuration);

            var productsProvider = new ProductsProvider(dbContext, null, mapper);

            var products = await productsProvider.GetProductAsync(-1);

            Assert.False(products.IsSuccess);
            Assert.Null(products.Product);
            Assert.NotNull(products.ErrorMessage);
        }

        private void CreateProducts(ProductsDbContext dbContext)
        {
            for (int i = 1; i < 10; i++)
            {
                dbContext.Products.Add(new Product
                {
                    Id = i,
                    Name = Guid.NewGuid().ToString(),
                    Inventory = i + 10,
                    Price = (decimal)(i * 3.54)
                });
            }

            dbContext.SaveChanges();
        }
    }
}
