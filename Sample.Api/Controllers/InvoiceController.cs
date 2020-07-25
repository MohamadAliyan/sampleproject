using Sample.Data.Entities;
using Sample.Service.Abstract;
using Sample.Service.Models;

namespace Sample.Api.Controllers
{
    public class InvoiceController : BaseApiController<Invoice, InvoiceServiceModel, InvoiceController>
    {
        private readonly IInvoiceService _invoiceService;
 
        public InvoiceController(IInvoiceService invoiceService
           ) : base(invoiceService)
        {
            _invoiceService = invoiceService;
         
        }




    }
}

       
   
    

