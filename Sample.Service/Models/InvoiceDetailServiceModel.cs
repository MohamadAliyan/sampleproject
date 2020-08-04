using Sample.Service.Models;

namespace Sample.Data.Entities
{
 
    public class InvoiceDetailServiceModel : BaseServiceModel
    {

        public long ProductId { get; set; }

        public int Count { get; set; }
        public decimal Price { get; set; }
        public virtual InvoiceServiceModel Invoice { get; set; }
        public  long InvoiceId { get; set; }


    }
   
}