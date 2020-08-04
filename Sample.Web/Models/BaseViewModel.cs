using System;

namespace Sample.Web.Models
{
    public class BaseViewModel
    {
        public long Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int CreatorId { get; set; }

    }
}
