using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCprojectMichal.Dal;
using System.Threading;
using MVCprojectMichal.Models;
using MVCprojectMichal.ViewModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace MVCprojectMichal.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        public ActionResult ErrorPage()
        {
            return View();
        }

        public ActionResult ErrorPageAddStud()
        {
            return View();
        }

        public ActionResult ErrorPageAddCourse()
        {
            return View();
        }

        public ActionResult ErrorPageAddGrade()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddCourses() ///gets the info
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCourses(Courses obj)//send info to db
        {
            CoursesDal dal = new CoursesDal();

            //list of all the courses
            List<Courses> CoursesObj = (from c in dal.courses
                                        select c).ToList<Courses>();

            List<Courses> LecturersCourses = (from c in dal.courses
                                              where c.LecturerId.Contains(obj.LecturerId)
                                              select c).ToList<Courses>();
            foreach (Courses lect in LecturersCourses)
            {
                List<Courses> Crs = (from c in CoursesObj
                                     where c.CourseId.Contains(lect.CourseId)
                                     select c).ToList<Courses>();

                //Courses crss = dal.courses.Find(obj.CourseName);
                foreach (Courses c in Crs)
                {
                    if (c.Day == obj.Day)
                    {
                        if (System.Convert.ToDecimal(c.StartHour) >= System.Convert.ToDecimal(obj.StartHour) &&
                        System.Convert.ToDecimal(c.StartHour) <= System.Convert.ToDecimal(obj.EndHour) ||
                        System.Convert.ToDecimal(c.EndHour) >= System.Convert.ToDecimal(obj.StartHour) &&
                        System.Convert.ToDecimal(c.EndHour) <= System.Convert.ToDecimal(obj.EndHour))
                        {
                            return RedirectToAction("ErrorPageAddCourse");
                        }
                    }

                }
            }

           // CoursesDal dal = new CoursesDal();
            if (dal.courses.Find(obj.CourseId)!=null)
            {
                return RedirectToAction("ErrorPage");
            }
            //Courses cr = new Courses();
            Courses courses = new Courses()
            {
                CourseName = obj.CourseName,
                CourseId = obj.CourseId,
                LecturerId = obj.LecturerId,
                ClassRoom = obj.ClassRoom,
                Day = obj.Day,
                Hours = obj.Hours,
                StartHour = obj.StartHour,
                EndHour = obj.EndHour
            };
            //CoursesDal dal = new CoursesDal();

            if (ModelState.IsValid)
            {
                try
                {
                    dal.courses.Add(obj);
                    dal.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["error"] = "The course already exist!\n"; // print error message
                    return View();
                }

            }
            return RedirectToAction("Home");
        }

        public ActionResult ShowCourses()//---> display table of all the courses in DB
        {
            List<Courses> cs = new List<Courses>();
            string mainconn = ConfigurationManager.ConnectionStrings["CoursesDal"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlQuery = "select Courses.CourseName, Courses.CourseId, Courses.LecturerId, Courses.ClassRoom,  Courses.Day, Courses.Hours From Courses";

            SqlCommand sqlcomm = new SqlCommand(sqlQuery, sqlconn);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                Courses crs = new Courses();
                crs.CourseName = dr["CourseName"].ToString();
                crs.CourseId = dr["CourseId"].ToString(); 
                crs.LecturerId = dr["LecturerId"].ToString();
                crs.ClassRoom = dr["ClassRoom"].ToString();
                crs.Day = dr["Day"].ToString();
                crs.Hours = dr["Hours"].ToString();
                cs.Add(crs);
            }
            return View(cs);
        }

        public ActionResult ShowExams()//---> display table of all the courses in DB
        {
            List<Exams> ex = new List<Exams>();
            string mainconn = ConfigurationManager.ConnectionStrings["ExamsDal"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlQuery = "select Exams.CourseName, Exams.CourseId, Exams.MoedADate, Exams.ClassA,  Exams.HoursA, Exams.MoedBDate, Exams.ClassB, Exams.HoursB From Exams";

            SqlCommand sqlcomm = new SqlCommand(sqlQuery, sqlconn);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                Exams exm = new Exams();
                exm.CourseName = dr["CourseName"].ToString();
                exm.CourseId = dr["CourseId"].ToString();
                exm.MoedADate = dr["MoedADate"].ToString();
                exm.ClassA = dr["ClassA"].ToString();
                exm.HoursA = dr["HoursA"].ToString();
                exm.MoedBDate = dr["MoedBDate"].ToString();
                exm.ClassB = dr["ClassB"].ToString();
                exm.HoursB = dr["HoursB"].ToString();
                
                ex.Add(exm);
            }
            return View(ex);
        }

        [HttpGet]
        public ActionResult Edit(string id)//--->Courses- gets course id and return all the info about it
        {
            CoursesDal dalC = new CoursesDal();
            List<Courses> objCourses = (from x in dalC.courses
                                          where x.CourseId.Contains(id)
                                          select x).ToList<Courses>();
            Courses cvm = new Courses();
            cvm = objCourses[0];
            return View(cvm);
        }

        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)//---> gets course id, delete the row of it and add new row with changes from the faculty
        {
            CoursesDal dalC = new CoursesDal();
            StudentsDal dalS = new StudentsDal();
            string x = collection["LecturerId"];
            string day = collection["Day"];
            string StartHour = collection["StartHour"];
            string EndHour = collection["EndHour"];
            //list of all the courses
            List<Courses> CoursesObj = (from c in dalC.courses
                                        select c).ToList<Courses>();
            
            
            List<Courses> LecturersCourses = (from c in dalC.courses
                                              where c.LecturerId.Contains(x)
                                              select c).ToList<Courses>();

            //----------check if the lecturer has another course in this time
            foreach (Courses lect in LecturersCourses)
            {
                List<Courses> Crs = (from c in CoursesObj
                                     where c.CourseId.Contains(lect.CourseId)
                                     select c).ToList<Courses>();

                //Courses crss = dal.courses.Find(obj.CourseName);
                foreach (Courses c in Crs)
                {
                    if (c.Day == day)
                    {
                        if (System.Convert.ToDecimal(c.StartHour) >= System.Convert.ToDecimal(StartHour) &&
                        System.Convert.ToDecimal(c.StartHour) <= System.Convert.ToDecimal(EndHour) ||
                        System.Convert.ToDecimal(c.EndHour) >= System.Convert.ToDecimal(StartHour) &&
                        System.Convert.ToDecimal(c.EndHour) <= System.Convert.ToDecimal(EndHour))
                        {
                            return RedirectToAction("ErrorPageAddCourse");
                        }
                    }

                }
            }
            //----------End check


            //----------check if there is a student with another course in this time
            List<Students> studentsCourses = (from s in dalS.students
                                              select s).ToList<Students>();
           
            foreach (Students std in studentsCourses)
            {
                List<Courses> Crs = (from c in CoursesObj
                                     where c.CourseId.Contains(std.CourseName)
                                     select c).ToList<Courses>();

                //Courses oneCourse = dalC.courses.Find(std.CourseName);
                foreach (Courses c in Crs)
                {
                    if (c.Day == day)
                    {
                        if (System.Convert.ToDecimal(c.StartHour) >= System.Convert.ToDecimal(StartHour) &&
                        System.Convert.ToDecimal(c.StartHour) <= System.Convert.ToDecimal(EndHour) ||
                        System.Convert.ToDecimal(c.EndHour) >= System.Convert.ToDecimal(StartHour) &&
                        System.Convert.ToDecimal(c.EndHour) <= System.Convert.ToDecimal(EndHour))
                        {
                            return RedirectToAction("ErrorPageAddStud");
                        }
                    }

                }

            }


            //CoursesDal dal = new CoursesDal();
            Courses cr = dalC.courses.Find(id);
            dalC.courses.Remove(cr);
            dalC.SaveChanges();

            Courses course = new Courses()
            {
                CourseName = collection["CourseName"],
                CourseId = collection["CourseId"],
                LecturerId = collection["LecturerId"],
                ClassRoom = collection["ClassRoom"],
                Day = collection["Day"],
                Hours = collection["Hours"],
                StartHour = collection["StartHour"],
                EndHour = collection["EndHour"]
            };

            if (ModelState.IsValid)
            {
                try
                {
                    (from c in dalC.courses
                     where c.CourseName == id
                     select c).ToList();
                    dalC.courses.Add(course);
                    dalC.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["error"] = "The course already exist!\n"; // print error message
                    return View();
                }
            }
            return View("Home");
        }

        [HttpGet]
        public ActionResult EditExam(string id)//---> gets Exam Id and return all the info about it
        {
            ExamsDal dalE = new ExamsDal();
            List<Exams> objExams = (from x in dalE.exams
                                    where x.CourseId.Contains(id)
                                    select x).ToList<Exams>();
            Exams cvm = new Exams();
            cvm = objExams[0];
            return View(cvm);
        }

        [HttpPost]
        public ActionResult EditExam(string id, FormCollection collection)//---> gets exam id, delete the row of it and add new row with changes from the faculty
        {
            ExamsDal dal = new ExamsDal();

            Exams ex = dal.exams.Find(id);
            dal.exams.Remove(ex);
            dal.SaveChanges();

            Exams exam = new Exams()
            {
                CourseName = collection["CourseName"],
                CourseId = collection["CourseId"],
                MoedADate = collection["MoedADate"],
                ClassA = collection["ClassA"],
                HoursA = collection["HoursA"],
                MoedBDate = collection["MoedBDate"],
                ClassB = collection["ClassB"],
                HoursB = collection["HoursB"]
            };

            if (ModelState.IsValid)
            {
                try
                {
                    (from c in dal.exams
                     where c.CourseId == id
                     select c).ToList();
                    dal.exams.Add(exam);
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

        public ActionResult ShowStudents()//---> display table of all the courses in DB
        {
            DateTime now = DateTime.Now;
            ExamsDal dalEx = new ExamsDal();

            List<Students> sd = new List<Students>();
            string mainconn = ConfigurationManager.ConnectionStrings["StudentsDal"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlQuery = "select Students.FirstName, Students.LastName, Students.Id,  Students.CourseName, Students.GradeMoedA, Students.GradeMoedB, Students.FinalGrade From Students";

            SqlCommand sqlcomm = new SqlCommand(sqlQuery, sqlconn);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            sda.Fill(dt);

             
            foreach (DataRow dr in dt.Rows)
            {
                Students std = new Students();
                string courseId = dr["CourseName"].ToString();

                std.FirstName = dr["FirstName"].ToString();
                std.LastName = dr["LastName"].ToString();
                std.Id = dr["Id"].ToString();
                std.CourseName = dr["CourseName"].ToString();
                List<Exams> ex = (from x in dalEx.exams
                                  where x.CourseId.Contains(courseId)
                                  select x).ToList<Exams>();
                foreach (Exams e in ex)
                {
                    DateTime MoedADate = DateTime.Parse(e.MoedADate);
                    DateTime MoedBDate = DateTime.Parse(e.MoedBDate);
                    int resultA = DateTime.Compare(MoedADate, now.Date);
                    if (resultA > 0)
                    {
                        std.GradeMoedA = "Has not yet been done";
                    }
                    else
                    {
                        std.GradeMoedA = dr["GradeMoedA"].ToString();
                    }
                    int resultB = DateTime.Compare(MoedBDate, now.Date);
                    if (resultB > 0)
                    {
                        std.GradeMoedB = "Has not yet been done";
                    }
                    else
                    {
                        std.GradeMoedB = dr["GradeMoedB"].ToString();
                    }
                }

                if (System.Convert.ToDecimal(dr["GradeMoedA"].ToString()) == -1 && System.Convert.ToDecimal(dr["GradeMoedB"].ToString()) == -1)
                {
                    std.FinalGrade = "Has not yet been done";
                }
                else
                {
                    if (System.Convert.ToDecimal(dr["GradeMoedA"].ToString()) > -1 && System.Convert.ToDecimal(dr["GradeMoedB"].ToString()) == -1)
                    {
                        std.FinalGrade = dr["GradeMoedA"].ToString();
                    }
                    else
                    {
                        std.FinalGrade = dr["GradeMoedB"].ToString();
                    }
                }
                //std.GradeMoedA = dr["GradeMoedA"].ToString();
                //std.GradeMoedB = dr["GradeMoedB"].ToString();
                //std.FinalGrade = dr["FinalGrade"].ToString();

                sd.Add(std);
            }
            return View(sd);
        }

        [HttpGet]
        public ActionResult EditStudent(string id)//---> gets course name and return all the info about it
        {
            StudentsDal dalS = new StudentsDal();
            List<Students> objStudents = (from x in dalS.students
                                       where x.Id.Contains(id)
                                       select x).ToList<Students>();
            Students svm = new Students();
            svm = objStudents[0];
            return View(svm);
        }

        [HttpPost]
        public ActionResult EditStudent(string id, string course, FormCollection collection)//---> gets course name, delete the row of it and add new row with changes from the faculty
        {
            DateTime now = DateTime.Now;
            StudentsDal dal = new StudentsDal();
            ExamsDal dalEx = new ExamsDal(); 
            Students sd = dal.students.Find(id, course);
            List<Exams> ex = (from x in dalEx.exams
                              where x.CourseId.Contains(sd.CourseName)
                              select x).ToList<Exams>();
            foreach(Exams e in ex)
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

        [HttpGet]
        public ActionResult AddStudents() ///gets the info
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddStudents(Students obj)//send info to db
        {
            StudentsDal dal = new StudentsDal();
            CoursesDal dalC = new CoursesDal();
            //list of of the same student and his courses
            List<Students> studentsCourses = (from s in dal.students
                                              where s.Id.Contains(obj.Id)
                                              select s).ToList<Students>();
            //list of all the courses
            List<Courses> CoursesObj = (from c in dalC.courses
                                        select c).ToList<Courses>();

            foreach(Students std in studentsCourses)
            {
                //only one course
                List<Courses> Crs = (from c in CoursesObj
                                     where c.CourseId.Contains(std.CourseName)
                                     select c).ToList<Courses>();
                Courses cr = dalC.courses.Find(obj.CourseName);
                foreach (Courses c in Crs)
                {
                    if(cr.Day == c.Day)
                    {
                        if (System.Convert.ToDecimal(cr.StartHour) >= System.Convert.ToDecimal(c.StartHour) &&
                        System.Convert.ToDecimal(cr.StartHour) <= System.Convert.ToDecimal(c.EndHour) ||
                        System.Convert.ToDecimal(cr.EndHour) >= System.Convert.ToDecimal(c.StartHour) &&
                        System.Convert.ToDecimal(cr.EndHour) <= System.Convert.ToDecimal(c.EndHour))
                        {
                            return RedirectToAction("ErrorPageAddStud");
                        }
                    }
                    
                }

            }
            Students students = new Students()
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Id = obj.Id,
                CourseName = obj.CourseName,
                GradeMoedA = obj.GradeMoedA,
                GradeMoedB = obj.GradeMoedB,
                FinalGrade = obj.FinalGrade
            };
            //StudentsDal dal = new StudentsDal();

            if (ModelState.IsValid)
            {
                try
                {
                    dal.students.Add(obj);
                    dal.SaveChanges();
                }
                catch (Exception)
                {
                    TempData["error"] = "The course already exist!\n"; // print error message
                    return View();
                }

            }
            return RedirectToAction("Home");
        }
    }
}