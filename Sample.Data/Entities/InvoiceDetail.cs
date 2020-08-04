namespace Sample.Data.Entities
{
 
    public class InvoiceDetail : BaseEntity
    {

        public long ProductId { get; set; }

        public int Count { get; set; }
        public decimal Price { get; set; }
        public virtual Invoice Invoice { get; set; }
        public  long InvoiceId { get; set; }


    }
   
}