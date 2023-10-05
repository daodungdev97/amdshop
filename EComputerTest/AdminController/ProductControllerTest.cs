using Ecom.DataAccess.Data;
using Ecom.DataAccess.Repository;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models.Models;
using Ecom.Models.ViewModels;
using ECommerceProject.Areas.Admin.Controllers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComputerTest.AdminController
{
    public class ProductControllerTest
    {
        private ProductController _productController;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductControllerTest()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _webHostEnvironment= A.Fake<IWebHostEnvironment>();
            _productController = new ProductController(_unitOfWork, _webHostEnvironment);
        }

        private ApplicationDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            if (context.ApplicationUsers.Count() <= 0)
            {
                context.ApplicationUsers.Add(
                  new ApplicationUser()
                  {
                      Id = "e34d0349-6ec5-4149-8a70-bb25ab4bfc99",
                      Name = "Dung",

                  });
                context.ShoppingCarts.Add(//giỏ hàng của 1 product
                   new ShoppingCart()
                   {
                       Id = 1,
                       ApplicationUserId = "e34d0349-6ec5-4149-8a70-bb25ab4bfc99",
                       Count = 2,
                       Price = 1,
                       ProductId = Guid.Parse("33005AD9-DDD8-4DBB-B025-669D781F4B5F"),

                   });
                context.Authors.Add(
                new Author()
                {
                    Id = 22,
                    Name = "AMD"

                });
                context.Categories.Add(
                 new Category()
                 {
                     Id = 33,
                     Name = "Mainboard",
                     DisplayOrder = 1

                 });
                context.Products.Add(
                   new Product()
                   {
                       Id = Guid.Parse("33005AD9-DDD8-4DBB-B025-669D781F4B5F"),
                       Title = "TEST",
                       Description = "TEST",
                       ISBN = "111",
                       AuthorId = 22,
                       CategoryId = 33,
                   });
                context.ProductImages.Add(
                new ProductImage()
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = "\\images\\product\\maina520m1.jpg",
                    ProductId = Guid.Parse("33005AD9-DDD8-4DBB-B025-669D781F4B5F")

                });
                context.SaveChanges();

            }

            return context;
        }


        [Fact]
        public void Upsert_POST_ReturnsActionResult()
        {
            // Arrange

            //DATABASE
            var tempdata = A.Fake<ITempDataDictionary>();
            var file = A.Fake<List<IFormFile>>();
            var dbContext = GetDatabaseContext();
            var unitOfWork = new UnitOfWork(dbContext);
            _productController = new ProductController(unitOfWork, _webHostEnvironment);
            _productController.TempData = tempdata;
            var productVM = new ProductVM()
            {

                Product = new Product()
                {//ADD NEW
                    Id = Guid.Empty,
                    Title = "TEST",
                    Description = "TEST",
                    ISBN = "111",
                    AuthorId = 22,
                    CategoryId = 33,
                },
                //{//UPDATE
                //    Id = Guid.NewGuid(),
                    
                //}
            };       
          

            // Act
            var result = _productController.Upsert(productVM, file);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().NotBeNull();

        }


    }
}
