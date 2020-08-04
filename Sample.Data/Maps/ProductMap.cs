using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.Data.Entities;

namespace Sample.Data.Maps
{
    public class ProductMap : BaseMap<Product>
    {
        public ProductMap(EntityTypeBuilder<Product> entityTypeBuilder) : base(entityTypeBuilder)
        {


        }
    }
}