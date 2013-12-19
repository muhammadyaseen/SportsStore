using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository respository;

        public AdminController(IProductRepository repo)
        {
            respository = repo;
        }

        public ViewResult Index()
        { 
            return View(respository.Products);
        }

        public ViewResult Edit(int productID)
        {
            Product product = respository.Products.FirstOrDefault(p => p.ProductID == productID);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null) {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }
                respository.SaveProduct(product);
                TempData["message"] = String.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else 
            {
                return View(product);
            }
        }

        [HttpPost]
        public ActionResult Delete(int prodId)
        {
            Product product = respository.Products.FirstOrDefault(p => p.ProductID == prodId);
            if (product != null)
            {
                respository.DeleteProduct(product);
                TempData["message"] = String.Format("{0} was deleted.", product.Name);
            }

            return RedirectToAction("Index");
        }
    }
}
