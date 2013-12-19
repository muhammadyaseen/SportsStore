using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        public int PageSize = 4; // TODO: Modify later
        
        private IProductRepository repository;

        public ProductController(IProductRepository productRepository)
        {
            repository = productRepository;
        }

        public ViewResult List(string category, int page = 1)
        {
            ProductListViewModel viewModel = new ProductListViewModel
            {
                Products = repository.Products
                                    .Where(p => category == null ? true:p.Category == category)
                                    .OrderBy(p => p.ProductID)
                                    .Skip((page - 1) * PageSize)
                                    .Take(PageSize),

                PagingInfo = new PagingInfo { 
                            CurrentPage = page,
                            ItemsPerPage = PageSize,
                            TotalItems = category == null ?
                                        repository.Products.Count() :
                                        repository.Products.Where(e => e.Category == category).Count()
                },
                CurrentCategory = category
             };
            return View( viewModel );
        }

        public FileContentResult GetImage(int prodId)
        {
            Product prod = repository.Products.FirstOrDefault(p => p.ProductID == prodId);
            if (prod != null)
                return File(prod.ImageData, prod.ImageMimeType);
            else
                return null;
        }

    }
}
