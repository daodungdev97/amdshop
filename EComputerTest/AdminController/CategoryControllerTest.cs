using Ecom.DataAccess.Data;
using Ecom.DataAccess.Repository;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models.Models;
using ECommerceProject.Areas.Admin.Controllers;
using ECommerceProject.Areas.Customer.Controllers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EComputerTest.AdminController
{
    public class CategoryControllerTest
    {
        private CategoryController _categoryController;
        private readonly IUnitOfWork _unitOfWork;


        public CategoryControllerTest()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _categoryController = new CategoryController( _unitOfWork);
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
        public void Upsert_ReturnsViewResult()
        {
            // Arrange

            //DATABASE,SESSION
            var dbContext = GetDatabaseContext();
            var unitOfWork = new UnitOfWork(dbContext);
            _categoryController = new CategoryController( unitOfWork);
            int? id = null;
            
            // Act
            var result = _categoryController.Upsert(id);

            // Assert
            result.Should().BeOfType<ViewResult>();
            result.Should().NotBeNull();

        }


        [Fact]
        public void Upsert_POST_ReturnsActionResult()
        {
            // Arrange

            //DATABASE
            var tempdata = A.Fake<ITempDataDictionary>();
            var dbContext = GetDatabaseContext();
            var unitOfWork = new UnitOfWork(dbContext);
            _categoryController = new CategoryController(unitOfWork);
            _categoryController.TempData = tempdata;
            var category = new Category()
            {
                Id = 0,//1
                Name="TEST32132"
            };
         
            // Act
            var result = _categoryController.Upsert(category);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().NotBeNull();

        }

        [Fact]
        public void Delete_ReturnsJsonResult()
        {
            // Arrange

            //DATABASE
            var dbContext = GetDatabaseContext();
            var unitOfWork = new UnitOfWork(dbContext);
            _categoryController = new CategoryController(unitOfWork);
            int id = 1;

            // Act
            var result = _categoryController.Delete(id);

            // Assert
            result.Should().BeOfType<JsonResult>();
            result.Should().NotBeNull();

        }

    }
}
