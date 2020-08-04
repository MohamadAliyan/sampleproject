using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sample.Data.Entities;
using Sample.Repository.Abstract;

namespace Sample.Repository.Concrete
{
    public class InvoiceDetailRepository : Repository<InvoiceDetail>, IInvoiceDetailRepository
    {
        private readonly DbSet<InvoiceDetail> _entity;
        public InvoiceDetailRepository(ApplicationContext context, ILogger<InvoiceDetail> logger) : base(context, logger)
        {
            this._entity = context.Set<InvoiceDetail>();
        }
    }
}