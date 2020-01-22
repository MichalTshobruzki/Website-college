using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVCprojectMichal.Models
{
    public class Courses
    {
        [Required]
        //[RegularExpression("^[A-Z][A-Za-z]*$", ErrorMessage = "Course name must start with big letter")]
        public string CourseName { get; set; }

        [Key]
        [Required]
        public string CourseId { get; set; }
        
        [Required]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Lecturer Id must be 6 digits (digits 0-9)")]
        public string LecturerId { get; set; }

        [Required]
        //[RegularExpression("^[A-Z][0-9][0-9][0-9]$", ErrorMessage = "Class Room must have big letter and 3 digits after (exmple: A101)")]
        public string ClassRoom { get; set; }

        [Required]
        //[RegularExpression("^[A-Z][a-z]*$", ErrorMessage = "Day must start with big letter")]
        public string Day { get; set; }

        [Required]
        //[RegularExpression("^(0[0-9]|1[0-9]|2[0-3]|[0-9]):[0-5][0-9]$", ErrorMessage = "Day must start with big letter")]
        public string Hours { get; set; }

        [Required]
        //[RegularExpression("^[0-9][0-9]$", ErrorMessage = "Hour must have 2 digits")]
        public string StartHour { get; set; }

        [Required]
        //[RegularExpression("^[0-9][0-9]$", ErrorMessage = "Hour must have 2 digits")]
        public string EndHour { get; set; }
    }

}