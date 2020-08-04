using Microsoft.Extensions.Caching.Memory;
using Sample.Data.Entities;
using Sample.Repository.Abstract;
using Sample.Service.Abstract;
using Sample.Service.Models;

namespace Sample.Service.Concrete
{
    public class ProductService : Service<Product, ProductServiceModel>, IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository, IMemoryCache memoryCache) : base(productRepository, memoryCache)
        {
            this._productRepository = productRepository;
        }

      

    }
}