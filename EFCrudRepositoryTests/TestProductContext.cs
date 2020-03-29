using Microsoft.EntityFrameworkCore;

namespace EFCrudRepositoryTests
{
    public class TestProductContext : DbContext
    {
        public TestProductContext(DbContextOptions<TestProductContext> options) : base(options)
        {

        }

        public virtual DbSet<TestProduct> Products { get; set; }

    }
}
