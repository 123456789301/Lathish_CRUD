using Lathish_CRUD.Models;
using Lathish_CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lathish_CRUD.Controllers
{
    public class SupplierController : Controller
    {
 
        private SupplierDataAccess dataAccess = new SupplierDataAccess();

         public ActionResult Index()
         {
            List<Supplier> suppliers = dataAccess.GetAllSuppliers();
            return View(suppliers);
         }

         public ActionResult Create()
         {
            ViewBag.ProductCategories = dataAccess.GetAllProductCategories();
            return View();
         }

         [HttpPost]
        public ActionResult Create(Supplier supplier, List<int> selectedProductCategories)
        {
            if (ModelState.IsValid)
            {
                dataAccess.AddSupplier(supplier, selectedProductCategories);
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategories = dataAccess.GetAllProductCategories();
            return View(supplier);
        }

         public ActionResult Edit(int id)
         {
            Supplier supplier = dataAccess.GetSupplierById(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductCategories = dataAccess.GetAllProductCategories();
             return View(supplier);
         }

         [HttpPost]
        public ActionResult Edit(Supplier supplier, List<int> selectedProductCategories)
        {
            if (ModelState.IsValid)
            {
                dataAccess.UpdateSupplier(supplier, selectedProductCategories);
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategories = dataAccess.GetAllProductCategories();
            return View(supplier);
        }

         public ActionResult Delete(int id)
         {
            Supplier supplier = dataAccess.GetSupplierById(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
         }

         [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            dataAccess.DeleteSupplier(id);
            return RedirectToAction("Index");
        }
    }
}