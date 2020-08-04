using System;
using System.Collections.Generic;
using Util;

namespace Sample.Web.Models
{
 
    public class InvoiceViewModel : BaseViewModel
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public List<InvoiceDetailViewModel> InvoiceDetails { get; set; }
        public string PersianDateTime
        {
            get => Date.GetPersianDateStr();
            set => this.Date = value.ConverToGregorianDate();
        }

    }
   
}