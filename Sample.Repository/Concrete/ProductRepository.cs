using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sample.Data.Entities;
using Sample.Repository.Abstract;

namespace Sample.Repository.Concrete
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly DbSet<Product> _entity;
        public ProductRepository(ApplicationContext context, ILogger<Product> logger) : base(context, logger)
        {
            this._entity = context.Set<Product>();
        }
    }
}