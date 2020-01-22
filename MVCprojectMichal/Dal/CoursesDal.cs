using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MVCprojectMichal.Models;

namespace MVCprojectMichal.Dal
{
    public class CoursesDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Courses>().ToTable("Courses");
        }
        public DbSet<Courses> courses { get; set; }

        public System.Data.Entity.DbSet<MVCprojectMichal.Models.Students> Students { get; set; }

        public System.Data.Entity.DbSet<MVCprojectMichal.Models.JoinStudents_Courses> JoinStudents_Courses { get; set; }
    }
}