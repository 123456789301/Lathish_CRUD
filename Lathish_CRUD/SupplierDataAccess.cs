using Lathish_CRUD.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Lathish_CRUD
{
    public class SupplierDataAccess
    {

        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

         public List<Supplier> GetAllSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT SupplierId, Name, GstNo, Address, ContactNo FROM Suppliers";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    suppliers.Add(new Supplier
                    {
                        SupplierId = (int)reader["SupplierId"],
                        Name = reader["Name"].ToString(),
                        GstNo = reader["GstNo"].ToString(),
                        Address = reader["Address"].ToString(),
                        ContactNo = reader["ContactNo"].ToString()
                    });
                }
            }

            return suppliers;
        }

         public void AddSupplier(Supplier supplier, List<int> selectedProductCategories)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Suppliers (Name, GstNo, Address, ContactNo) OUTPUT INSERTED.SupplierId VALUES (@Name, @GstNo, @Address, @ContactNo)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", supplier.Name);
                cmd.Parameters.AddWithValue("@GstNo", supplier.GstNo);
                cmd.Parameters.AddWithValue("@Address", supplier.Address);
                cmd.Parameters.AddWithValue("@ContactNo", supplier.ContactNo);

                conn.Open();
                int supplierId = (int)cmd.ExecuteScalar();

                 foreach (var categoryId in selectedProductCategories)
                {
                    string categoryQuery = "INSERT INTO SupplierProductCategoriess (SupplierID, CategoryID) VALUES (@SupplierId, @ProductCategoryId)";
                    SqlCommand categoryCmd = new SqlCommand(categoryQuery, conn);
                    categoryCmd.Parameters.AddWithValue("@SupplierId", supplierId);
                    categoryCmd.Parameters.AddWithValue("@ProductCategoryId", categoryId);
                    categoryCmd.ExecuteNonQuery();
                }
            }
        }

         public Supplier GetSupplierById(int id)
        {
            Supplier supplier = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT SupplierId, Name, GstNo, Address, ContactNo FROM Suppliers WHERE SupplierId = @SupplierId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SupplierId", id);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    supplier = new Supplier
                    {
                        SupplierId = (int)reader["SupplierId"],
                        Name = reader["Name"].ToString(),
                        GstNo = reader["GstNo"].ToString(),
                        Address = reader["Address"].ToString(),
                        ContactNo = reader["ContactNo"].ToString()
                    };
                }
            }

            return supplier;
        }

         public void UpdateSupplier(Supplier supplier, List<int> selectedProductCategories)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                 
                //string query = "UPDATE Suppliers SET Name = @Name, GstNo = @GstNo, Address = @Address, ContactNo = @ContactNo WHERE SupplierId = @SupplierId";
                SqlCommand cmd = new SqlCommand("sp_supplier_update", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SupplierId", supplier.SupplierId);
                cmd.Parameters.AddWithValue("@Name", supplier.Name);
                cmd.Parameters.AddWithValue("@GstNo", supplier.GstNo);
                cmd.Parameters.AddWithValue("@Address", supplier.Address);
                cmd.Parameters.AddWithValue("@ContactNo", supplier.ContactNo);

                conn.Open();
                cmd.ExecuteNonQuery();

                SqlCommand deleteCmd = new SqlCommand("DELETE FROM SupplierProductCategoriess WHERE SupplierId = @SupplierId", conn);
                deleteCmd.Parameters.AddWithValue("@SupplierId", supplier.SupplierId);
                deleteCmd.ExecuteNonQuery();

                foreach (var categoryId in selectedProductCategories)
                {
                    string categoryQuery = "INSERT INTO SupplierProductCategoriess (SupplierID, CategoryID) VALUES (@SupplierId, @ProductCategoryId)";
                    SqlCommand categoryCmd = new SqlCommand(categoryQuery, conn);
                    categoryCmd.Parameters.AddWithValue("@SupplierId", supplier.SupplierId);
                    categoryCmd.Parameters.AddWithValue("@ProductCategoryId", categoryId);
                    categoryCmd.ExecuteNonQuery();
                }
            }
        }

         public void DeleteSupplier(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_supplier_delete", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SupplierId", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

         public List<ProductCategory> GetAllProductCategories()
        {
            List<ProductCategory> categories = new List<ProductCategory>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT CategoryID, CategoryName FROM ProductCategories";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new ProductCategory
                    {
                        ProductCategoryId = (int)reader["CategoryID"],
                        Name = reader["CategoryName"].ToString()
                    });
                }
            }

            return categories;
        }
    }
}
