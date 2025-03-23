using Lathish_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lathish_CRUD.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer

        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-DIHT49T\SQLEXPRESS; Initial Catalog=DBLathish_CRUD; Integrated Security=True;");

        [HttpGet]
        public ActionResult Index()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_Custview", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            return View(dt);
        }

        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_Custinsert", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", customer.Name);
            cmd.Parameters.AddWithValue("@Contact_No", customer.contact);
            cmd.Parameters.AddWithValue("@PAN_Card", customer.pancard);
            cmd.Parameters.AddWithValue("@Address", customer.Address);
            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");

        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            Customer customer = new Customer();
            DataTable dt = new DataTable();
            con.Open();
            string qry = "select * from customertable where id = @id";
            SqlCommand cmd = new SqlCommand(qry, con);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.SelectCommand.Parameters.AddWithValue("@id", id);
            sda.Fill(dt);
            if(dt.Rows.Count>0)
            {
                customer.id = Convert.ToInt32(dt.Rows[0][0]);
                customer.Name = dt.Rows[0][1].ToString();
                customer.contact = Convert.ToInt32(dt.Rows[0][2]);
                customer.pancard = Convert.ToInt32(dt.Rows[0][3]);
                customer.Address = dt.Rows[0][4].ToString();
                return View(customer);

            }
            else
            {
                return RedirectToAction("Index");


            }

        }

        // POST: Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(Customer customer)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_Custupdate", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", customer.id);
            cmd.Parameters.AddWithValue("@Name", customer.Name);
            cmd.Parameters.AddWithValue("@Contact_No", customer.contact);
            cmd.Parameters.AddWithValue("@PAN_Card", customer.pancard);
            cmd.Parameters.AddWithValue("@Address", customer.Address);
            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

     
        public ActionResult Delete(int id)
        {
            Customer customer = new Customer();
            DataTable dt = new DataTable();
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_Custdelete", con);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id",id);
            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }
    }
}
