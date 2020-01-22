using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MVCprojectMichal.Models;

namespace MVCprojectMichal.Dal
{
    public class StudentsDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Students>().ToTable("Students");
        }
        public DbSet<Students> students { get; set; }

        public System.Data.Entity.DbSet<MVCprojectMichal.Models.Courses> Courses { get; set; }
    }
}