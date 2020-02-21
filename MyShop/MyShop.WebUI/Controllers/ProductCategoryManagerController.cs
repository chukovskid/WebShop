using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        InMemoryRepository<ProductCategory> context;

        public ProductCategoryManagerController()
        {
            context = new InMemoryRepository<ProductCategory>();
        }
        // GET: ProductManager
        //INDEX
        public ActionResult Index()
        {
            List<ProductCategory> products = context.Collection().ToList();
            return View(products);
        }
        //CREATE
        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }
            else
            {
                context.Insert(productCategory);
                context.Commit();

                return RedirectToAction("Index");
            }

        }
        //EDIT
        public ActionResult Edit(string Id)
        {
            ProductCategory productCategory = context.Find(Id);
            if (productCategory == null)
            {
                return HttpNotFound(); 
            }
            else
            {
                return View(productCategory);
            }
        }

        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string Id)
        {
            ProductCategory productCategoryToEdit = context.Find(Id);

            if (productCategoryToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productCategory);
                }

                productCategoryToEdit.Category = productCategory.Category;

                context.Commit();

                return RedirectToAction("Index");
            }
        }
        //DELETE
        public ActionResult Delete(string Id)
        {
            ProductCategory productCatogoryToDelete = context.Find(Id);

            if (productCatogoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productCatogoryToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            ProductCategory productCatogoryToDelete = context.Find(Id);

            if (productCatogoryToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}