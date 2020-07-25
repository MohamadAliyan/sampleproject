using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.Data.Entities;

namespace Sample.Data.Maps
{
    public class InvoiceMap : BaseMap<Invoice>
    {
        public InvoiceMap(EntityTypeBuilder<Invoice> entityTypeBuilder) : base(entityTypeBuilder)
        {


        }
    }
}