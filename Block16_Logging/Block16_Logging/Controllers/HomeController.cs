using Block16_Logging.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Block16_Logging.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logs()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gibz\Documents\logging_intrusion_detection.mdf;Integrated Security=True;Connect Timeout=30";

            SqlCommand cmdCredentials = new SqlCommand();
            cmdCredentials.CommandText = "SELECT * FROM [dbo].[UserLog] ul JOIN [dbo].[User] u ON ul.UserId = u.Id ORDER BY ul.Createdon DESC";
            cmdCredentials.Connection = con;

            con.Open();

            SqlDataReader reader = cmdCredentials.ExecuteReader();

            if (reader.HasRows)
            {
                List<HomeControllerViewModel> viewModel = new List<HomeControllerViewModel>();
                while (reader.Read())
                {
                    HomeControllerViewModel logEntry = new HomeControllerViewModel();

                    // 10 = Id
                    // 0 = LogId
                    // 7 = CreatedOn

                    logEntry.UserId = reader.GetValue(10).ToString();
                    logEntry.LogId = reader.GetValue(0).ToString();
                    logEntry.LogCreatedOn = reader.GetValue(7).ToString();

                    viewModel.Add(logEntry);
                }

                return View(viewModel);
            }
            else
            {
                ViewBag.message = "No Entry found";
                return View();
            }
        }

        [HttpPost]
        public ActionResult DoLogin()
        {
            var username = Request["username"];
            var password = Request["password"];

            var ip = Request.ServerVariables["REMOTE_ADDR"];
            var platform = Request.Browser.Platform;
            var browser = Request.UserAgent;

            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gibz\Documents\logging_intrusion_detection.mdf;Integrated Security=True;Connect Timeout=30";

            // Made this like the tutorial, but this is insecure against SQL-Injection
            SqlCommand cmdCredentials = new SqlCommand();
            cmdCredentials.CommandText = "SELECT [Id] FROM [dbo].[User] WHERE [Username] = '" + username + "' AND [Password] = '" + password + "'";
            cmdCredentials.Connection = con;

            con.Open();

            SqlDataReader readerCredentials = cmdCredentials.ExecuteReader();

            if (readerCredentials.HasRows)
            {
                var user_id = 0;
                while (readerCredentials.Read())
                {
                    user_id = readerCredentials.GetInt32(0);
                    break;
                }

                con.Close();
                con.Open();

                SqlCommand cmdUserUsingUsualBrowser = new SqlCommand();
                cmdUserUsingUsualBrowser.CommandText = "SELECT [Id] FROM [dbo].[UserLog] WHERE [UserId] = '" +
                    user_id + "' AND [IP] LIKE '" + ip.Substring(0, 2) + "%' AND [Browser] LIKE '" + platform + "%'";

                cmdUserUsingUsualBrowser.Connection = con;

                SqlDataReader readerUsualBrowser = cmdUserUsingUsualBrowser.ExecuteReader();

                if (!readerUsualBrowser.HasRows)
                {
                    con.Close();
                    con.Open();

                    SqlCommand logCmd = new SqlCommand();
                    logCmd.CommandText = "INSERT INTO [dbo].[UserLog] (UserId, IP, Action, Result, CreatedOn, Browser, AdditionalInformation) Values('" + user_id + "', '" +
                                            ip + "', 'login', 'success', GETDATE(), '" + platform + "', 'other browser')";

                    logCmd.Connection = con;
                    logCmd.ExecuteReader();
                }
                else
                {
                    con.Close();
                    con.Open();

                    SqlCommand logCmd = new SqlCommand();
                    logCmd.CommandText = "INSERT INTO [dbo].[UserLog] (UserId, IP, Action, Result, CreatedOn, Browser) Values('" + user_id + "', '" +
                                            ip + "', 'login', 'success', GETDATE(), '" + platform + "')";

                    logCmd.Connection = con;
                    logCmd.ExecuteReader();
                }
            }
            else
            {
                con.Close();
                con.Open();

                SqlCommand cmdUserIdByName = new SqlCommand();

                cmdUserIdByName.CommandText = "SELECT [Id] FROM [dbo].[User] WHERE [Username] = '" + username + "'";

                SqlDataReader reader = cmdUserIdByName.ExecuteReader();
                if (reader.HasRows)
                {
                    var userid = 0;
                    while (readerCredentials.Read())
                    {
                        userid = readerCredentials.GetInt32(0);
                        break;
                    }

                    con.Close();
                    con.Open();

                    SqlCommand failedLogCmd = new SqlCommand();
                    failedLogCmd.CommandText = "SELECT COUNT(ID) FROM [dbo].[UserLog] WHERE [UserId] = '" + userid + "' " +
                        "AND [Result] = 'failed' AND CAST(CreatedOn As date) = '" + System.DateTime.Now.ToShortDateString().Substring(0, 10) + "'";
                    failedLogCmd.Connection = con;
                    SqlDataReader failedLoginCount = failedLogCmd.ExecuteReader();

                    int attemps = 0;
                    if (failedLoginCount.HasRows)
                    {
                        while (readerCredentials.Read())
                        {
                            attemps = readerCredentials.GetInt32(0);
                            break;
                        }
                    }

                    if (attemps >= 5 || password.Length < 4 || password.Length > 20)
                    {
                        // block user!
                    }

                    con.Close();
                    con.Open();

                    SqlCommand logCmd = new SqlCommand();
                    logCmd.CommandText = "INSERT INTO [dbo].[UserLog] (UserId, IP, Action, Result, CreatedOn, Browser) " +
                        "VALUES('" + userid + "', '" + ip + "', login', 'failed', GETDATE(), '" + platform + "')";
                    logCmd.Connection = con;
                    logCmd.ExecuteReader();

                    ViewBag.Message = "No User Found";
                }
                else
                {
                    con.Close();
                    con.Open();

                    SqlCommand logCmd = new SqlCommand();
                    logCmd.CommandText = "INSERT INTO [dbo].[UserLog] (UserId, IP, Action, Result, CreatedOn, AdditionalInformation, Browser) " +
                        "VALUES(0, '" + ip + "', login', 'failed', GETDATE(), 'No user found', '" + platform + "')";

                    logCmd.Connection = con;
                    logCmd.ExecuteReader();

                    ViewBag.Message = "No User Found";
                }
            }
            con.Close();
            return RedirectToAction("Logs", "Home");
        }

        private void PrintLog()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\gibz\Documents\logging_intrusion_detection.mdf;Integrated Security=True;Connect Timeout=30";

            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT [Id], [Username], [Password] FROM [dbo].[User]";
            cmd.Connection = con;

            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine("{0}\t{1}", reader.GetInt32(0), reader.GetString(1));
                }
            }
            else
            {
                Console.WriteLine("No Rows found in the Database");
            }
        }
    }
}