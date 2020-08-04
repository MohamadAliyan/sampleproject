namespace Sample.Web.Models
{
 
    public class InvoiceDetailViewModel : BaseViewModel
    {

        public long ProductId { get; set; }
 public int Count { get; set; }
        public decimal Price { get; set; }
        public InvoiceViewModel Invoice { get; set; }
        public  long InvoiceId { get; set; }


    }
   
}