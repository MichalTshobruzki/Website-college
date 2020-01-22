using MVCprojectMichal.Dal;
using MVCprojectMichal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCprojectMichal.Controllers
{
    public class LecturerController : Controller
    {
        // GET: Lecturer
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult ErrorPageAddGrade()
        {
            return View();
        }
        

        public ActionResult Schedule()
        {
            string id = Session["Id"].ToString();
            //string LectName = "select Users.CourseName, Courses.Day, Courses.Hours From Courses where Courses.LecturerId=" + id;

            List<JoinStudents_Courses> jc = new List<JoinStudents_Courses>();
            string mainconn = ConfigurationManager.ConnectionStrings["CoursesDal"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlQuery = "select Courses.CourseName, Courses.CourseId, Courses.Day, Courses.Hours From Courses where Courses.LecturerId=" + id;

            SqlCommand sqlcomm = new SqlCommand(sqlQuery, sqlconn);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                JoinStudents_Courses jcobj = new JoinStudents_Courses();
                jcobj.CourseName = dr["CourseName"].ToString();
                jcobj.CourseId = dr["CourseId"].ToString();
                jcobj.Day = dr["Day"].ToString();
                jcobj.Hours = dr["Hours"].ToString();
                jc.Add(jcobj);
            }
            return View(jc);
        }

        public ActionResult Details(string id)
        {
            List<Students> sd = new List<Students>();
            string mainconn = ConfigurationManager.ConnectionStrings["StudentsDal"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            //string sqlQuery = "select Students.FirstName, Students.LastName, Students.Id, Students.CourseName, Students.GradeMoedA, Students.GradeMoedB, Students.FinalGrade From Students where Students.CourseName="+id;
            string sqlQuery = "select Students.FirstName, Students.LastName, Students.Id, Students.CourseName, Students.GradeMoedA, Students.GradeMoedB, FinalGrade From Students where Students.CourseName=" + id;
            SqlCommand sqlcomm = new SqlCommand(sqlQuery, sqlconn);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                Students std = new Students();
                std.FirstName = dr["FirstName"].ToString();
                std.LastName = dr["LastName"].ToString();
                std.Id = dr["Id"].ToString();
                std.CourseName = dr["CourseName"].ToString();
              
                if (System.Convert.ToDecimal(dr["FinalGrade"].ToString())!= -1)
                {
                    std.FinalGrade = dr["FinalGrade"].ToString();
                }
               
                sd.Add(std);
            }
            return View(sd);
        }

        [HttpGet]
        public ActionResult EditGrade(string id, string course)//---> gets course id and name and return all the info about it
        {
            StudentsDal dalS = new StudentsDal();
            List<Students> objStudents = (from x in dalS.students
                                          where x.Id.Contains(id) && x.CourseName.Contains(course)
                                          select x).ToList<Students>();
            Students svm = new Students();
            svm = objStudents[0];
            return View(svm);
        }

        [HttpPost]
        public ActionResult EditGrade(string id, string course, FormCollection collection)//---> gets course id and name, delete the row of it and add new row with changes from the faculty
        {
            StudentsDal dal = new StudentsDal();
            Students sd = dal.students.Find(id,course);
            ExamsDal dalEx = new ExamsDal();

            DateTime now = DateTime.Now;
      
           // Students sd1 = dal.students.Find(id, course);
            List<Exams> ex = (from x in dalEx.exams
                              where x.CourseId.Contains(sd.CourseName)
                              select x).ToList<Exams>();
            foreach (Exams e in ex)
            {
                DateTime MoedADate = DateTime.Parse(e.MoedADate);
                DateTime MoedBDate = DateTime.Parse(e.MoedBDate);
                int resultA = DateTime.Compare(MoedADate, now.Date);
                if (resultA > 0)
                {
                    return RedirectToAction("ErrorPageAddGrade");
                }
                int resultB = DateTime.Compare(MoedBDate, now.Date);
                if (resultB > 0)
                {
                    return RedirectToAction("ErrorPageAddGrade");
                }
            }



            dal.students.Remove(sd);
            dal.SaveChanges();

            Students stud = new Students()
            {
                FirstName = collection["FirstName"],
                LastName = collection["LastName"],
                Id = collection["Id"],
                CourseName = collection["CourseName"],
                GradeMoedA = collection["GradeMoedA"],
                GradeMoedB = collection["GradeMoedB"],
                FinalGrade = collection["FinalGrade"]
            };

            if (ModelState.IsValid)
            {
                try
                {
                    (from s in dal.students
                     where s.Id == id
                     select s).ToList();
                    dal.students.Add(stud);
                    dal.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["error"] = "The course already exist!\n"; // print error message
                    return View();
                }
            }
            return View("Home");
        }
    }
}