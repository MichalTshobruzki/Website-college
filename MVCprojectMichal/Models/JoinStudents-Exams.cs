using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCprojectMichal.Models
{
    public class JoinStudents_Exams
    {
        [Key]
        [Required]
        public string CourseName { get; set; }

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