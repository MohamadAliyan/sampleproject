using System;
using System.Collections;
using System.Collections.Generic;
using Sample.Service.Models;

namespace Sample.Data.Entities
{
 
    public class InvoiceServiceModel : BaseServiceModel
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public List<InvoiceDetailServiceModel> InvoiceDetails { get; set; }


    }

}