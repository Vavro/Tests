namespace EfInsertBigData
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=TestDbConnString")
        {
        }

        public virtual DbSet<TP_CEKAJICIZMENY> TP_CEKAJICIZMENY { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TP_CEKAJICIZMENY>()
                .Property(e => e.ID)
                .HasPrecision(10, 0);
        }
    }
}
