using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MVCprojectMichal.Models;

namespace MVCprojectMichal.Dal
{
    public class UsersDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Users>().ToTable("Users");
        }
        public DbSet<Users> users { get; set; }
    }
}