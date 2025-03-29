using webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace webapi.Persistence.Repositories.Implementation
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

    }
}
