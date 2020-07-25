using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sample.Data.Entities;
using Sample.Repository.Abstract;

namespace Sample.Repository.Concrete
{
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        private readonly DbSet<Invoice> _entity;
        public InvoiceRepository(ApplicationContext context, ILogger<Invoice> logger) : base(context, logger)
        {
            this._entity = context.Set<Invoice>();
        }
    }
}