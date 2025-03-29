using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Models
{
    public abstract class BaseEntity
    {
        public virtual int Id { get; set; }

        [Column("UpdatedDate", TypeName = "DateTime")]
        public DateTime? UpdatedDate { get; set; }

        [Column("InsertedDate", TypeName = "DateTime")]
        public DateTime? InsertedDate { get; set; }

        public string InsertedUser { get; set; }

        public string UpdatedUser { get; set; }
    }
}
