using Microsoft.EntityFrameworkCore;

namespace Final.Data
{
    public class FinalWebContext : DbContext
    {
        public FinalWebContext(DbContextOptions<FinalWebContext> options) : base(options)
        {

        }
    }
}
