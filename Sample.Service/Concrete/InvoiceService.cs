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
        private readonly IInvoiceDetailRepository _invoiceDetailRepository;
        public InvoiceService(IInvoiceRepository invoiceRepository, IInvoiceDetailRepository invoiceDetailRepository, IMemoryCache memoryCache) : base(invoiceRepository, memoryCache)
        {
            this._invoiceRepository = invoiceRepository;
            this._invoiceDetailRepository = invoiceDetailRepository;
        }


        public override void Insert(InvoiceServiceModel serviceModel, int currentUserId)
        {
            var invoice = new Invoice
            {
                Date = serviceModel.Date,
                Number = serviceModel.Number,

            };
            var id=_invoiceRepository.InsertAndGetId(invoice,currentUserId);
            foreach (var item in serviceModel.InvoiceDetails)
            {
                var invoiceDetail = new InvoiceDetail()
                {
                    InvoiceId = id,
                    Count = item.Count,
                    Price = item.Price,
                    ProductId = item.ProductId
                };
                _invoiceDetailRepository.Insert(invoiceDetail,currentUserId);
            }

        }
    }
}