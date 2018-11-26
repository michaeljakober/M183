using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Block12_SQLInjection.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        { 
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoLogin()
        {
            var username = Request["username"];
            var password = Request["password"];
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\gibz\\Documents\\sql_xss_injection.mdf\";Integrated Security=True;Connect Timeout=30";

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dataReader;

            cmd.CommandText = "SELECT [Id] ,[username] ,[password] FROM [dbo].[User] WHERE [username] = '"+username+"' AND [password] = '" + password + "'";
            cmd.Connection = con;

            con.Open();
            dataReader = cmd.ExecuteReader();
            if (dataReader.HasRows)
            {
                ViewBag.Message = "success";
                while (dataReader.Read())
                {
                    ViewBag.Message += dataReader.GetInt32(0) + " " + dataReader.GetString(1) + " " + dataReader.GetString(2);
                }
            }
            else
            {
                ViewBag.Message = "nothing to read of";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DoFeedback()
        {
            var feedback = Request["feedback"];
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\gibz\\Documents\\sql_xss_injection.mdf\";Integrated Security=True;Connect Timeout=30";

            SqlCommand cmd = new SqlCommand();
            SqlDataReader dataReader;

            cmd.CommandText = "INSERT INTO [dbo].[Feedback] SET [feedback = '" + feedback + "'"
            cmd.Connection = con;

            con.Open();
            dataReader = cmd.ExecuteReader();
            if (dataReader.HasRows)
            {
                ViewBag.Message = "success";
                while (dataReader.Read())
                {
                    ViewBag.Message += dataReader.GetInt32(0) + " " + dataReader.GetString(1);
                }
            }
            else
            {
                ViewBag.Message = "nothing to read of";
            }
            return View();
        }
    }
}