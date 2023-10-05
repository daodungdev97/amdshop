using Ecom.DataAccess.Data;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models.Models;
using Ecom.Models.ViewModels;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace ECommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWorke, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWorke;
        }
        public IActionResult Index()
        {       
            return View();
        }

        public IActionResult Upsert(Guid? id)
        {

            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category
               .GetAll().Select(u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Id.ToString()

               }),
                AuthorList = _unitOfWork.Author
               .GetAll().Select(u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Id.ToString()

               }),
                Product = new Product()
            };
            if (id == null)
            {//create 
                return View(productVM);
            }
            else
            {//update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id,includeProperties:"ProductImages");
                return View(productVM);
            }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, List<IFormFile> files)
        {

            if (ModelState.IsValid)
            {
                if (productVM.Product.Id == Guid.Empty)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }

                _unitOfWork.Save();

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\products\product-" + productVM.Product.Id;
                        string finalPath = Path.Combine(wwwRootPath, productPath);

                        if (!Directory.Exists(finalPath))//check xem có đường dẫn không.
                            Directory.CreateDirectory(finalPath);

                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);//tạo file
                        }

                        ProductImage productImage = new()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId = productVM.Product.Id,
                        };

                        if (productVM.Product.ProductImages == null)
                            productVM.Product.ProductImages = new List<ProductImage>();

                        productVM.Product.ProductImages.Add(productImage);

                    }

                    _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();

                }

                TempData["success"] = "Product created/updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category
                          .GetAll().Select(u => new SelectListItem
                          {
                              Text = u.Name,
                              Value = u.Id.ToString()

                          });
                productVM.AuthorList = _unitOfWork.Author
               .GetAll().Select(u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Id.ToString()

               });
                return View(productVM);
            }

        }


        public IActionResult DeleteImage(Guid imageId)
        {
            var imageToBeDeleted = _unitOfWork.ProductImage.Get(u => u.Id == imageId);
            Guid productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath =
                                   Path.Combine(_webHostEnvironment.WebRootPath,
                                   imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.ProductImage.Remove(imageToBeDeleted);
                _unitOfWork.Save();

                TempData["success"] = "Deleted successfully";
            }

            return RedirectToAction(nameof(Upsert), new { id = productId });
        }




        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category,Author").ToList();
            return Json(new { data = products });
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            } 

            string productPath = @"images\products\product-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }


            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        
        
        #endregion

    }
}
