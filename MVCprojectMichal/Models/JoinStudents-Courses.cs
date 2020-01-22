using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCprojectMichal.Models
{
    public class JoinStudents_Courses
    {
        [Required]
        public string CourseName { get; set; }

        [Key]
        [Required]
        public string CourseId { get; set; }

        [Required]
        public string Day { get; set; }

        [Required]
        public string Hours { get; set; }
    }
}