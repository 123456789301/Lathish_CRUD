using Lathish_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lathish_CRUD.Controllers
{
    public class ProductController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

         [HttpPost]    
        public ActionResult Search(string searchTerm)
        {
            List<Search> searchResults = SearchProducts(searchTerm); // Changed the variable name to be more descriptive
            ViewBag.Products = searchResults; // Use the same name as in the controller

            return View();
        }


        private List<Search> SearchProducts(string searchTerm)
        {
            List<Search> products = new List<Search>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                                 SELECT 
                                     pc.CategoryID AS id, 
                                     pc.CategoryName AS Categoryname,
                                     s.Name as Suppliername
                                 FROM 
                                     Suppliers s
                                 LEFT JOIN 
                                     SupplierProductCategoriess p ON s.SupplierID = p.SupplierID
                                 INNER JOIN 
                                     ProductCategories pc ON p.CategoryID = pc.CategoryID
                                 WHERE 
                                 pc.CategoryName LIKE @SearchTerm OR s.Name LIKE @SearchTerm";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Search
                    {

                        supplierName = reader["Suppliername"].ToString(),
                        CategoryName = reader["Categoryname"].ToString(),
                     
                    });
                }
            }

            return products;
        }
    }
}
