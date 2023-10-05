using Ecom.DataAccess.Data;
using Ecom.DataAccess.Repository;
using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models.Models;
using ECommerceProject.Areas.Customer.Controllers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EComputerTest.CustomerController
{
    public class HomeControllerTest
    {
        private HomeController _homeController;
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public HomeControllerTest()
        {
            _logger = A.Fake<ILogger<HomeController>>();
            _unitOfWork = A.Fake<IUnitOfWork>();
            _homeController = new HomeController(_logger, _unitOfWork);
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
            _homeController = new HomeController(_logger, unitOfWork);
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier,"e34d0349-6ec5-4149-8a70-bb25ab4bfc99"),
            };
            var identity = new ClaimsIdentity(claims);
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(identity);
            httpContext.Session = session;       
            _homeController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };


            // Act
            var result = _homeController.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
            result.Should().NotBeNull();

        }


        [Fact]
        public void Index_Post_ReturnsViewResult()
        {
            // Arrange
            int page = 1;
            string searchField = "";
            int? categoryId = null;
            var dbContext = GetDatabaseContext();
            var unitOfWork = new UnitOfWork(dbContext);        
            _homeController = new HomeController(_logger, unitOfWork);

            // Act
            var result = _homeController.Index(page, searchField, categoryId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model); 
        }

        [Fact]
        public void Detail_ReturnsViewResult()
        {
            // Arrange
            Guid productId = Guid.Parse("33005AD9-DDD8-4DBB-B025-669D781F4B5F");
            var dbContext = GetDatabaseContext();
            var unitOfWork = new UnitOfWork(dbContext);
            _homeController = new HomeController(_logger, unitOfWork);

            // Act
            var result = _homeController.Detail(productId);

            // Assert
            result.Should().BeOfType<ViewResult>();
            result.Should().NotBeNull();
        }

        [Fact]
        public void Detail_Post_ReturnsActionResult()
        {
            // Arrange         
            //IDENTITY,SESSION
            var session = A.Fake<ISession>();
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier,"e34d0349-6ec5-4149-8a70-bb25ab4bfc99"),
            };
            var identity = new ClaimsIdentity(claims);
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(identity);
            httpContext.Session = session;

            var tempData = A.Fake<ITempDataDictionary>();

          
            //*******SHOPPING********         
            var shopping = new ShoppingCart();     
            shopping.Count = 2;
            //CASE UPDATE , tracked cần disable       
            //shopping.ProductId = Guid.Parse("33005AD9-DDD8-4DBB-B025-669D781F4B5F");

            //CASE CREATE
            shopping.ProductId = Guid.Parse("99005AD9-DDD8-4DBB-B025-669D781F4B5F");

            //DATABASE
            var dbContext = GetDatabaseContext();
            var unitOfWork = new UnitOfWork(dbContext);
            _homeController = new HomeController(_logger, unitOfWork);
            _homeController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
            _homeController.TempData=tempData;

            // Act
            var result = _homeController.Detail(shopping);
           

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            result.Should().NotBeNull();
           
        }


        public class TestSession : ISession
        {
            private readonly Dictionary<string, byte[]> _sessionData = new Dictionary<string, byte[]>();

            public string Id => throw new NotImplementedException();

            public bool IsAvailable => throw new NotImplementedException();

            public IEnumerable<string> Keys => _sessionData.Keys;

            public void Clear()
            {
                _sessionData.Clear();
            }

            public Task CommitAsync(CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public Task LoadAsync(CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public void Remove(string key)
            {
                _sessionData.Remove(key);
            }

            public void Set(string key, byte[] value)
            {
                _sessionData[key] = value;
            }

            public bool TryGetValue(string key, out byte[] value)
            {
                return _sessionData.TryGetValue(key, out value);
            }
        }

    }
}
