using Microsoft.Extensions.Caching.Memory;
using Sample.Data.Entities;
using Sample.Repository.Abstract;
using Sample.Service.Abstract;
using Sample.Service.Models;

namespace Sample.Service.Concrete
{
    public class InvoiceDetailService : Service<InvoiceDetail, InvoiceDetailServiceModel>, IInvoiceDetailService
    {
        private readonly IInvoiceDetailRepository _invoiceDetailRepository;
        public InvoiceDetailService(IInvoiceDetailRepository invoiceRepository, IMemoryCache memoryCache) : base(invoiceRepository, memoryCache)
        {
            this._invoiceDetailRepository = invoiceRepository;
        }

      

    }
}