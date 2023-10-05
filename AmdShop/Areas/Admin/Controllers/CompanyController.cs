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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompanyController(IUnitOfWork unitOfWorke, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWorke;
        }
        public IActionResult Index()
        {
         //   List<Company> companys = _unitOfWork.Company.GetAll().ToList();
            return View();
        }

        public IActionResult Upsert(Guid? id)
        {
           
            if (id == null)
            {//create
                return View(new Company());
            }
            else
            {//update
                Company company = _unitOfWork.Company.Get(u => u.Id == id);
                return View(company);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Upsert(Company company)
        {

            if (ModelState.IsValid)
            {
               
                if(company.Id == Guid.Empty)
                {
                    _unitOfWork.Company.Add(company);
                    TempData["success"] = "Company Added Suceessful";

                }
                else
                {
                    _unitOfWork.Company.Update(company);
                    TempData["success"] = "Company Updated Suceessful";

                }
                _unitOfWork.Save();
               
                return RedirectToAction("Index");
            } 
            else
            {
             
                return View(company);
            }

        }
     

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> Companys = _unitOfWork.Company.GetAll().ToList();
            return Json(new {data = Companys });
        }

        [ValidateAntiForgeryToken] 
        [HttpDelete]
        public IActionResult Delete(Guid? id)
        {
            var CompanyToDelete = _unitOfWork.Company.Get(u => u.Id == id);
            if (CompanyToDelete == null)
            {
                return Json(new {success =false, message ="Error while deleting"});
            }

            _unitOfWork.Company.Remove(CompanyToDelete);
            _unitOfWork.Save();

           
            return Json(new { success=true, message = "Delete successful" });
        }
        #endregion

    }
}
