using Microsoft.EntityFrameworkCore;
using WInnovator.Data;

namespace WInnovatorTest.Data
{
    public class DbContextTest
    {
        protected readonly ApplicationDbContext _applicationTestDbContext;

        protected DbContextTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "WInnovatorDb")
                .Options;

            _applicationTestDbContext = new ApplicationDbContext(options);
        }


    }
}
