using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.Data.Entities;

namespace Sample.Data.Maps
{
    public class BaseMap<T> where T : BaseEntity
    {
        public BaseMap(EntityTypeBuilder<T> entityBuilder) 
        {
            entityBuilder.HasKey(t => t.Id);
        }
    }
}
