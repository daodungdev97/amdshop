using Ecom.DataAccess.Data;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.DataAccess.Repository;
using Ecom.Models.Models;
using ECommerceProject.Areas.Customer.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace EComputerTest.CustomerController
{
    public class CartControllerTest
    {
        private CartController _cartController;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;


        public CartControllerTest()
        {         
            _unitOfWork = A.Fake<IUnitOfWork>();
            _emailSender = A.Fake<IEmailSender>();
            _cartController = new CartController( _unitOfWork,_emailSender);
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
        public void Index_Get_ReturnsViewResult()
        {
            // Arrange

            //DATABASE,SESSION
            var dbContext = GetDatabaseContext();
            var unitOfWork = new UnitOfWork(dbContext);
            var session = A.Fake<ISession>();

            //IDENTITY
            _cartController = new CartController(unitOfWork, _emailSender);
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier,"e34d0349-6ec5-4149-8a70-bb25ab4bfc99"),
            };
            var identity = new ClaimsIdentity(claims);
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(identity);
            httpContext.Session = session;
            _cartController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = _cartController.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
            result.Should().NotBeNull();

        }

        [Fact]
        public void Summary_ReturnsViewResult()
        {
            // Arrange

            //DATABASE
            var dbContext = GetDatabaseContext();
            var unitOfWork = new UnitOfWork(dbContext);
    
            //IDENTITY
            _cartController = new CartController(unitOfWork, _emailSender);
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier,"e34d0349-6ec5-4149-8a70-bb25ab4bfc99"),
            };
            var identity = new ClaimsIdentity(claims);
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(identity);

            _cartController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = _cartController.Summary();

            // Assert
            result.Should().BeOfType<ViewResult>();
            result.Should().NotBeNull();

        }


    }
}
