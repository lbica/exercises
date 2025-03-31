using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    [Table("dim_customers")]
    public class Customer : BaseEntity
    {
        // this is the surrogate (technical) key
        [Key]
        [Column("customer_key")]
        public override int Id { get => base.Id; set => base.Id = value; }

        // this is the business key of a customer
        [Required]
        [Column("customer_id")]
        public string CustomerId { get; set; }

        [Column("country")]
        public String Country { get; set; }


     }
}
