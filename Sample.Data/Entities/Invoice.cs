using System;
using System.Collections;
using System.Collections.Generic;

namespace Sample.Data.Entities
{
 
    public class Invoice : BaseEntity
    {
        public int Number { get; set; }
        public DateTime Date { get; set; }
 
    }
   
}