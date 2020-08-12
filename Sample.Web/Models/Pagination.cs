using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sample.Web.Models
{
    public class Pagination
    {
        public int CurrentPageNumber { get; set; }
        public int TotalCount { get; set; }
        public int TotalPageCount { get; set; }
        public string Url { get; set; }
        public QueryString QueryString { get; set; }
    }
}
