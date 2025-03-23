using Lathish_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Lathish_CRUD.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
   
        public ActionResult Register()
        {
            return View();
        }

         [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(fm_Login model)
        {
            if (ModelState.IsValid)
            {
                 model.PasswordHash = HashPassword(model.Password);

                using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-DIHT49T\SQLEXPRESS; Initial Catalog=DBLathish_CRUD; Integrated Security=True;"))
                {
                    conn.Open();
 
                    using (SqlCommand cmd = new SqlCommand("sp_Register", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", model.Name);
                        cmd.Parameters.AddWithValue("@ContactNo", model.ContactNo);
                        cmd.Parameters.AddWithValue("@Email", model.Email);
                        cmd.Parameters.AddWithValue("@PasswordHash", model.PasswordHash);

                        cmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Login");
            }

            return View(model);
        }

         public ActionResult Login()
        {
            return View();
        }

         [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-DIHT49T\SQLEXPRESS; Initial Catalog=DBLathish_CRUD; Integrated Security=True;"))
            {
                conn.Open();
                 using (SqlCommand cmd = new SqlCommand("sp_Login", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        string storedPasswordHash = reader["PasswordHash"].ToString();

                         if (VerifyPassword(password, storedPasswordHash))
                        {
                            Session["UserID"] = reader["UserID"];
                            Session["UserName"] = reader["Name"];
                            return RedirectToAction("Index", "Supplier");
                        }
                        else
                        {
                            ViewBag.Message = "Invalid email or password.";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Invalid email or password.";
                    }
                }
            }

            return View();
        }

         private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

         private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredPasswordHash = HashPassword(enteredPassword);
            return enteredPasswordHash == storedHash;
        }

         public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("fm_Login");
        }
    }
}



 
 