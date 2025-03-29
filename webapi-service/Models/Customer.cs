using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    public class Customer : BaseEntity
    {
        // this is the surrogate (technical) key
        [Key]
        [Column("Customer_Id")]
        public override int Id { get => base.Id; set => base.Id = value; }

        // this is the business key of a customer
        [Required]
        public string CustomerId { get; set; }

        [Column("Country")]
        public string Country { get; set; }


     }
}
