using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCprojectMichal.Models
{
    public class Exams
    {
        [Required]
        public string CourseName { get; set; }

        [Key]
        [Required]
        public string CourseId { get; set; }

        [Required]
        public string MoedADate { get; set; }

        [Required]
        public string ClassA { get; set; }

        [Required]
        public string HoursA { get; set; }

        [Required]
        public string MoedBDate { get; set; }

        [Required]
        public string ClassB { get; set; }

        [Required]
        public string HoursB { get; set; }
    }
}