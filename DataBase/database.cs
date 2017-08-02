namespace DataBase
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class database : DbContext
    {
        public database()
            : base("name=database")
        {
        }

        public virtual DbSet<user> users { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<user>()
        //        .Property(e => e.username)
        //        .IsFixedLength();

        //    modelBuilder.Entity<user>()
        //        .Property(e => e.password)
        //        .IsFixedLength();

        //    modelBuilder.Entity<user>()
        //        .Property(e => e.role)
        //        .IsFixedLength();
        //}
    }
}
