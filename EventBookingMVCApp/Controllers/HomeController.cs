using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using EventBookingMVCApp.Models;
using System.Net.Mail;
using System.Net;
using System.Web.Helpers;
using System.Text;

namespace EventBookingMVCApp.Controllers
{
    public class HomeController : Controller
    {
        string connectionstring = "Data Source=LAPTOP-7UIHE5VR\\SQLEXPRESS;Initial Catalog=EventBookingManagement;Integrated Security=True";
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult EventDisplay()
        {
            List<Program> programs = new List<Program>();
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("EventList", sqlConnection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Program program = new Program
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    Name = Convert.ToString(reader["Name"]),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    Date = Convert.ToDateTime(reader["date"]),
                };
                programs.Add(program);
            }
            return View(programs);
        }
        public ActionResult AdminEntry()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminEntry(string Username, string Password)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("AdminEntry", sqlConnection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Username", Username);
            command.Parameters.AddWithValue("@Password", Password);
            command.ExecuteNonQuery();
            sqlConnection.Close();
            return View();
        }
        public ActionResult EventAdd()
        {
            ViewBag.result = "";
            return View();
        }
        [HttpPost]
        public ActionResult EventAdd(string Name, string Quantity, DateTime Date)
        {
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlCommand command = new SqlCommand("EventSave", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Quantity", Quantity);
            command.Parameters.AddWithValue("@Date", Convert.ToDateTime(Date));
            command.ExecuteNonQuery();
            connection.Close();
            ViewBag.result = "Record Saved";
            return View();
        }
        public ActionResult CSV()
        {
            List<Program> programs = new List<Program>();
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlCommand command = new SqlCommand("EventList", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Program p = new Program
                {
                    ID = Convert.ToInt32(reader["ID"]),
                    Name = Convert.ToString(reader["Name"]),
                    Quantity = Convert.ToInt32(reader["Quantity"]),
                    Date = Convert.ToDateTime(reader["Date"]),
                };
                programs.Add(p);
            }
            var builder = new StringBuilder();
            builder.AppendLine("ID,Name,Quantity,Date");
            foreach (var i in programs)
            {
                builder.AppendLine($"{i.ID},{i.Name},{i.Quantity},{i.Date}");
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "csv", "Event.csv");
        }
        public ActionResult BuyTicket()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BuyTicket(string useremail)
        {
            string Subject = "Ticket purchase confirmation";
            string Body = " Your Booking has been confirmed.<br/><br/>Thank you";
            WebMail.Send(useremail, Subject, Body);
            ViewBag.msg = "Booking Success";
            return View();
        }
        public ActionResult UserSave()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserSave(string Name, string Mobile, string Email, string Tickets)
        {
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            SqlCommand command = new SqlCommand("UserSave", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@Mobile", Convert.ToInt64(Mobile));
            command.Parameters.AddWithValue("@Email", Email);
            command.Parameters.AddWithValue("@Tickets", Convert.ToInt32(Tickets));
            command.ExecuteNonQuery();
            connection.Close();
            string subject = "Ticket purchase confirmation";
            string body = " Your Booking has been confirmed.<br/><br/>Thank you";
            WebMail.Send(Email, subject, body);
            return View();

        }
    }
}