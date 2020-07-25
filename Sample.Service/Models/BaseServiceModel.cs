using System;

namespace Sample.Service.Models
{
    public class BaseServiceModel
    {
        public long Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int CreatorId { get; set; }

    }
}
