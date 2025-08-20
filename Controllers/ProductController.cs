using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;
        public int PageSize = 2;

        public ProductController( IProductRepository repository )
        {
            _repository = repository;
        }

        public ViewResult List( string category, int productPage = 1 )
        {
            var productsListViewModel = new ProductsListViewModel();
            productsListViewModel.Products = _repository.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductID)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize);

            productsListViewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = category == null ?
                    _repository.Products.Count() :
                    _repository.Products.Where(e => e.Category == category).Count()
            };
            productsListViewModel.CurrentCategory = category;

            #region
            //new ProductsListViewModel
            //{
            //    Products = _repository.Products
            //        .Where(p => category == null || p.Category == category)
            //        .OrderBy(p => p.ProductID)
            //        .Skip((productPage - 1) * PageSize)
            //        .Take(PageSize),
            //    PagingInfo = new PagingInfo
            //    {
            //        CurrentPage = productPage,
            //        ItemsPerPage = PageSize,
            //        TotalItems = _repository.Products.Count()
            //    },
            //    CurrentCategory = category
            //}
            #endregion
            return View(productsListViewModel);
        }
    }
}
