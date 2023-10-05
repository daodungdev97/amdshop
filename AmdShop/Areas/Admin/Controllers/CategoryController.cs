using Ecom.DataAccess.Data;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models.Models;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;  

namespace ECommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;  

        public CategoryController(IUnitOfWork unitOfWorke) 
        {

            _unitOfWork = unitOfWorke;
        }
        public IActionResult Index()
        {                           
            return View();
        }

       
        public IActionResult Upsert(int? id)
        {

            if (id == null )
            {//create
                return View(new Category());
            }
            else
            {//update
                Category obj = _unitOfWork.Category.Get(u => u.Id == id);
                return View(obj);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Upsert(Category category)
        {

            if (ModelState.IsValid)
            {

                if (category.Id == null ||category.Id == 0)
                {
                    _unitOfWork.Category.Add(category);
                    TempData["success"] = "Category Added Suceessful";

                }
                else
                {
                    _unitOfWork.Category.Update(category);
                    TempData["success"] = "Category Updated Suceessful";

                }
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            else
            {

                return View(category);
            }

        }



        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(int? id)
        {
            List<Category> Categories = _unitOfWork.Category.GetAll().ToList();
            return Json(new { data = Categories });
        }

        [HttpDelete]
        [ValidateAntiForgeryToken] 
        public IActionResult Delete(int? id)
        {
            var CategoryToDelete = _unitOfWork.Category.Get(u => u.Id == id);
            if (CategoryToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Category.Remove(CategoryToDelete);
            _unitOfWork.Save();


            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion


    }
}
