namespace EfInsertBigData
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TransactionProtocolContext : DbContext
    {
        public TransactionProtocolContext()
            : base("name=TestDbConnString")
        {
        }

        public virtual DbSet<TP_CEKAJICIZMENY> TP_CEKAJICIZMENY { get; set; }
        public virtual DbSet<TP_ZPRACOVANE> TP_ZPRACOVANE { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TP_CEKAJICIZMENY>()
                .Property(e => e.ID)
                .HasPrecision(10, 0);

            modelBuilder.Entity<TP_ZPRACOVANE>()
                .Property(e => e.ID)
                .HasPrecision(10, 0);
        }
    }
}
