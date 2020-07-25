using System;
using System.ComponentModel.DataAnnotations;

namespace Sample.Data.Entities
{
    public class BaseEntity
    {

        public long Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [Required]
        public int CreatorId { get; set; }
        [Required]
        public int LastModifierId { get; set; }
    }
}
