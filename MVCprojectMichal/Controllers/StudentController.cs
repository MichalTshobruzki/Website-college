using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCprojectMichal.Models;
using MVCprojectMichal.ViewModel;
using MVCprojectMichal.Dal;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace MVCprojectMichal.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Schedule()
        {
            var id = Session["Id"];
            List<JoinStudents_Courses> jc = new List<JoinStudents_Courses>();
            string mainconn = ConfigurationManager.ConnectionStrings["CoursesDal"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            //string sqlQuery = "select Courses.CourseName, Courses.Day, Courses.Hours From Courses inner join Students On Students.CourseName=Courses.CourseName";
            string sqlQuery = "select Courses.CourseName, Courses.CourseId, Courses.Day, Courses.Hours From Courses ,Students where Students.Id=" + id + " and Students.CourseName = Courses.CourseId";

            SqlCommand sqlcomm = new SqlCommand(sqlQuery, sqlconn);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach(DataRow dr in dt.Rows)
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

        public ActionResult Exams_Schedule()
        {
            var id = Session["Id"];
            List<JoinStudents_Exams> jc = new List<JoinStudents_Exams>();
            string mainconn = ConfigurationManager.ConnectionStrings["ExamsDal"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlQuery = "select Exams.CourseName, Exams.MoedADate, Exams.ClassA, Exams.HoursA, Exams.MoedBDate, Exams.ClassB, Exams.HoursB From Exams ,Students where Students.Id=" + id + " and Students.CourseName = Exams.CourseId";
            SqlCommand sqlcomm = new SqlCommand(sqlQuery, sqlconn);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                JoinStudents_Exams jcobj = new JoinStudents_Exams();
                jcobj.CourseName = dr["CourseName"].ToString();
                jcobj.MoedADate = dr["MoedADate"].ToString();
                jcobj.ClassA = dr["ClassA"].ToString();
                jcobj.HoursA = dr["HoursA"].ToString();
                jcobj.MoedBDate = dr["MoedBDate"].ToString();
                jcobj.ClassB = dr["ClassB"].ToString();
                jcobj.MoedBDate = dr["HoursB"].ToString();
                jc.Add(jcobj);
            }
            return View(jc);
        }
    }
}