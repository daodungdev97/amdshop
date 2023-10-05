using Ecom.DataAccess.Repository.IRepository;
using Ecom.Models.Models;
using Ecom.Models.ViewModels;
using Ecom.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stripe.Checkout;
using System.Security.Claims;

namespace ECommerceProject.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;


        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()//không hiểu
            };

            IEnumerable<ProductImage> productImages = _unitOfWork.ProductImage.GetAll();

            foreach (var cart in ShoppingCartVM.ShoppingCartList) //mỗi ShopingCart là 1 thông tin của 1 product
            {
                cart.Product.ProductImages = productImages.Where(u => u.ProductId == cart.Product.Id).ToList();
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);//ShopingCart.count
            }

            return View(ShoppingCartVM);
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                //remove that from cart

                _unitOfWork.ShoppingCart.Remove(cartFromDb);
                HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId, tracked: true);//no track default



            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart
             .GetAll(u => u.ApplicationUserId == cartFromDb.ApplicationUserId).Count() - 1);//need tracked
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

     

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()
            };

            //lấy từ đây truyền xuống dưới
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;



            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product");//lấy các sản phầm đơn lẻ trong list

            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            //các thuộc tính khác của  ShoppingCartVM tự map với bindproperty           
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);


            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == Guid.Empty)
            {
                //it is a regular customer 
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                //it is a COMPANY STAFF USER    
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            //chỉ chạy khi không phải là COMPANY STAFF
            if (applicationUser.CompanyId.GetValueOrDefault() == Guid.Empty)
            {
                //it is a regular customer account and we need to capture payment
                //stripe logic
                var domain = "https://localhost:7001/";
                //Request.Scheme + "://" + Request.Host.Value + "/"
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                    CancelUrl = domain + "customer/cart/index",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach (var item in ShoppingCartVM.ShoppingCartList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Title
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }


                var service = new SessionService();
                Session session = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);

            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
        }


        public IActionResult OrderConfirmation(Guid id)
        {

            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
            if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                //this is an order by customer

                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")//đã thanh toán
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
                HttpContext.Session.Clear();
            }

            _emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - AMD  Shop",
               $@"<html>
             <head>
                   
            </head>
            <body  style=""font-size:16px;
                font-family: 'Sniglet', cursive; background-image: linear-gradient(to bottom, #cffafe, #fee2e2);
                background-repeat: no-repeat;
                background-size: cover;
                background-position: center; 
                "">
               <div style=""display: grid; grid-template-columns: repeat(6, 1fr); gap: 20px;"">
                    <div style=""grid-column: span 5;"">
                        <img style=""width: 50%;"" src=""https://bom.so/kqH2hD"" alt="""">
                    </div>
                    <div style=""grid-column: span 6;"">
                        <p style=""font-size: 26px"">Dear {orderHeader.ApplicationUser.Name},</p>
                       <p style=""font-size: 22px"">New Order Created - {orderHeader.Id}</p>
                        <p>Chúng tôi sẽ liên lạc với bạn ngay!</p>
                        <p style=""font-style: italic; font-weight: bold;"">AMD  Shop </p>
                    </div>
                </div>
     
               </body>
            </html>");

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
                .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id);
        }

        [HttpPost]
        public IActionResult SendMail(string email, string name)
        {

            _emailSender.SendEmailAsync(email, "Subcribed- AMD  Shop ",
               $@"<html>
             <head>
                   
            </head>
            <body  style=""font-size:16px;
                font-family: 'Sniglet', cursive; background-image: linear-gradient(to bottom, #cffafe, #fee2e2);
                background-repeat: no-repeat;
                background-size: cover;
                background-position: center; 
                "">
               <div style=""display: grid; grid-template-columns: repeat(6, 1fr); gap: 20px;"">
                    <div style=""grid-column: span 5;"">
                        <img style=""width: 50%;"" src=""https://bom.so/kqH2hD"" alt="""">
                    </div>
                    <div style=""grid-column: span 6;"">
                        <p style=""font-size: 26px"">Dear {name},</p>
                        <p>Cảm ơn bạn đã đăng kí thông tin mới nhất  ở AMD Shop</p>
                        <p>Chúng tôi sẽ liên lạc với bạn ngay!</p>
                        <p style=""font-style: italic; font-weight: bold;"">AMD  Shop </p>
                    </div>
                </div>
     
               </body>
            </html>");

            TempData["success"] = "Send Email Suceessful";
            return RedirectToAction("Index", "Home");
        }




    }
}

