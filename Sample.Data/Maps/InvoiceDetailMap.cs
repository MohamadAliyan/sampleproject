using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sample.Data.Entities;

namespace Sample.Data.Maps
{
    public class InvoiceDetailMap : BaseMap<InvoiceDetail>
    {
        public InvoiceDetailMap(EntityTypeBuilder<InvoiceDetail> entityTypeBuilder) : base(entityTypeBuilder)
        {


        }
    }
}