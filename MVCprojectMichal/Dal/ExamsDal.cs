using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MVCprojectMichal.Models;

namespace MVCprojectMichal.Dal
{
    public class ExamsDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Exams>().ToTable("Exams");
        }
        public DbSet<Exams> exams { get; set; }
    }
   
}