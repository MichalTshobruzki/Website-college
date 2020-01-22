using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCprojectMichal.Models;
using MVCprojectMichal.ViewModel;
using MVCprojectMichal.Dal;

namespace MVCprojectMichal.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View("Submit");
        }


        public ActionResult LogOff()
        {
            //FormsAuthentication.SignOut();
            Session["Id"] = null;
           // Session["Admin"] = null;
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Submit()
        {
            UsersDal dal = new UsersDal();
            string userName = Request.Form["UserName"].ToString();
            string password = Request.Form["Password"].ToString();
            List<Users> objUsers = (from usr in dal.users where usr.UserName == userName select usr).ToList<Users>();
            if (objUsers.Count<Users>() != 0)
            {
                if (objUsers[0].Password == password)
                {
                    Session["Id"] = objUsers[0].Id;
                    switch (objUsers[0].Permission)
                    {
                        case "1":
                            return RedirectToAction("Home", "Admin");
                            //return View();
                        case "2":
                            return RedirectToAction("Home", "Lecturer");
                        case "3":
                            return RedirectToAction("Home", "Student"); 
                            //return RedirectToAction("ShowStudentSearch", "Student");
                    }
                }
            }
            ViewBag.LoginError = "User Name or Password invalid";
            //return RedirectToAction("StudentPage", "Student");
            return View("Submit");
        }
    }
}