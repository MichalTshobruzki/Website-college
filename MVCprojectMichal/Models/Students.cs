using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCprojectMichal.Models
{
    public class Students
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Key]
        [Column(Order = 0)]
        [Required]
        public string Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [Required]
        public string CourseName { get; set; }

        [Required]
        public string GradeMoedA { get; set; }

        [Required]
        public string GradeMoedB { get; set; }

        [Required]
        public string FinalGrade { get; set; }    
    }
}