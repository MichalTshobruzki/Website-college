using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCprojectMichal.Models;

namespace MVCprojectMichal.ViewModel
{
    public class ExamsViewModel
    {
        public Exams exam { get; set; }

        public List<Exams> exams { get; set; }
    }
}