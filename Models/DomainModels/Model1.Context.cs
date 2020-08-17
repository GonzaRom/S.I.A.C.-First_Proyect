using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace S.I.A.C.Models.DomainModels
{
    public class dbSIACEntities : DbContext
    {
        public dbSIACEntities()
            : base("name=dbSIACEntities")
        {
        }

        public virtual DbSet<category> category { get; set; }
        public virtual DbSet<module> module { get; set; }
        public virtual DbSet<operations> operations { get; set; }
        public virtual DbSet<people> people { get; set; }
        public virtual DbSet<priority> priority { get; set; }
        public virtual DbSet<rol> rol { get; set; }
        public virtual DbSet<rolOperations> rolOperations { get; set; }
        public virtual DbSet<status> status { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<ticket> ticket { get; set; }
        public virtual DbSet<ticketHistory> ticketHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    }
}