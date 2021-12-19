using BaseTypeService.Model;
using Microsoft.EntityFrameworkCore;

namespace BaseTypeService.Data
{
    public class BaseTypeContext : DbContext
    {
        public BaseTypeContext (DbContextOptions<BaseTypeContext> options) : base(options)
        {

        }
        public DbSet<BaseRootType> BaseRootType { get; set; }
        public DbSet<BaseBranchType> BaseBranchType { get; set; }
    }
}
