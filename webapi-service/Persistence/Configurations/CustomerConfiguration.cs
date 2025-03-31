
using webapi.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;


namespace WebApi.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // set id as value generated on insert
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(x => x.CustomerId)
                .IsUnique();

            //add default New record: ghost record for undefined
            builder.HasData(
            new Customer()
            {
                Id = -1,
                CustomerId = "Undefined",
                Country = "Undefined",
                //InsertedDate = DateTime.Now,
                //UpdatedDate = DateTime.Now,
                //InsertedUser = "SYSTEM",
                //UpdatedUser = "SYSTEM"
            });

            //add default New record: ghost record for unknown
            builder.HasData(
            new Customer()
            {
                Id = -2,
                CustomerId = "Unknown",
                Country = "Unknown",
                //InsertedDate = DateTime.Now,
                //UpdatedDate = DateTime.Now,
                //InsertedUser = "SYSTEM",
                //UpdatedUser = "SYSTEM"
            });

        }
    }
}