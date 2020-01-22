using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCprojectMichal.Models;


namespace MVCprojectMichal.ViewModel
{
    public class UsersViewModel
    {
        public Users user { get; set; }

        public List<Users> users { get; set; }
    }
}