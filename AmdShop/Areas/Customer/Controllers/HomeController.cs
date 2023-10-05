using Ecom.DataAccess.Data;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models.Models;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics; 
using System.Security.Claims;

namespace ECommerceProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            int pageSize = 12;
            int page = 1;
            int totalItems = 0;
            int totalPages = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            { 
               var cart=_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value);
                int numberCart = cart.Count();
            
             HttpContext.Session.SetInt32(SD.SessionCart, numberCart);
                              
            }

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages,Author");
            totalItems = productList.Count();
            totalPages = (int)Math.Ceiling((double)totalItems / pageSize); //tổng số trang
            productList=productList.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            });
            ViewBag.TotalPages = totalPages;

            IEnumerable<Product> productsBestSeller= _unitOfWork.Product.GetAll(includeProperties: "ProductImages,Author")
                .OrderByDescending(p => p.BuyCount).Take(3);

            ViewBag.ProductsBestSeller = productsBestSeller;
            return View(productList);
        }

        [HttpPost]
        public IActionResult SearchByCollection(int idCollection)
        {
            int pageSize = 12;
            int page = 1;
            int totalItems = 0;
            int totalPages = 0;

         
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(x =>x.CollectionId == idCollection,includeProperties: "Category,ProductImages,Author");
            totalItems = productList.Count();
            totalPages = (int)Math.Ceiling((double)totalItems / pageSize); //tổng số trang
            productList = productList.Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
            });
            ViewBag.TotalPages = totalPages;

            IEnumerable<Product> productsBestSeller = _unitOfWork.Product.GetAll(includeProperties: "ProductImages,Author")
             .OrderByDescending(p => p.BuyCount).Take(3);

            ViewBag.ProductsBestSeller = productsBestSeller;
            return View("Index", productList);
        }

        [HttpPost]
        public IActionResult Index(int page , string? searchField , int? searchCategoryId )
        {        
            int pageSize = 12;
            int totalItems = 0;
            int totalPages = 0;
            IEnumerable<Product> productList;
            if ( string.IsNullOrEmpty(searchField) && searchCategoryId == null)
            {
                productList = _unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages,Author");                
                totalItems = productList.Count();
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize); 
                productList = productList.Skip((page - 1) * pageSize).Take(pageSize);
            }
            else 
            {
                productList = _unitOfWork.Product.GetAll(
                p =>(searchCategoryId == null || p.CategoryId == searchCategoryId)
                && (string.IsNullOrEmpty(searchField) || p.Title.Contains(searchField)),
                includeProperties: "Category,ProductImages,Author");
                totalItems = productList.Count();
                totalPages = (int)Math.Ceiling((double)totalItems / pageSize); 
                productList = productList.Skip((page - 1) * pageSize).Take(pageSize);
            }

            ViewBag.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),             
            });
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            IEnumerable<Product> productsBestSeller = _unitOfWork.Product.GetAll(includeProperties: "ProductImages,Author")
               .OrderByDescending(p => p.BuyCount).Take(3);

            ViewBag.ProductsBestSeller = productsBestSeller;
            return View(productList);
        }

        public IActionResult Detail(Guid productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category,ProductImages,Author"),
                Count = 1,
                ProductId = productId
            };

            IEnumerable<Product> productsInSameCategory = _unitOfWork.Product.GetAll(
         p => p.Category.Id == cart.Product.CategoryId && p.Id != productId,
         includeProperties: "ProductImages,Author").Take(3);

            ViewBag.ProductsInSameCategory = productsInSameCategory;

            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Detail(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;//lấy từ identityname

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
            u.ProductId == shoppingCart.ProductId);

            if (cartFromDb != null )  
            {
                //shopping cart exists
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }
            else
            {
                // Id = 0,ProductId chưa có sẽ tạo mới 
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            TempData["success"] = "Cart updated successfully";

            return RedirectToAction(nameof(Index));//không back về view lại yêu cầu property
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}