using Microsoft.Extensions.Caching.Memory;
using Sample.Data.Entities;
using Sample.Repository.Abstract;
using Sample.Service.Abstract;
using Sample.Service.Models;

namespace Sample.Service.Concrete
{
    public class InvoiceService : Service<Invoice, InvoiceServiceModel>, IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        public InvoiceService(IInvoiceRepository invoiceRepository, IMemoryCache memoryCache) : base(invoiceRepository, memoryCache)
        {
            this._invoiceRepository = invoiceRepository;
        }

      

    }
}