using BulkyBook.DataAcces.Repository;
using BulkyBook.DataAcces.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utillity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public ShoppingCardViewModel ShoppingCardViewModel { get; set; }
        public double OrderTotal { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity; // de id van de ingelogde user
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCardViewModel = new ShoppingCardViewModel()
            {
                ListCart = unitOfWork.ShoppingCard.GetAll(u => u.ApplicationUserId == claim.Value, icludeProperties: "Product"),
                OrderHeader = new()
            };
            foreach (var cart in ShoppingCardViewModel.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                ShoppingCardViewModel.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }
            return View(ShoppingCardViewModel);
        }
        public IActionResult Summary ()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity; // de id van de ingelogde user
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCardViewModel = new ShoppingCardViewModel()
            {
                ListCart = unitOfWork.ShoppingCard.GetAll(u => u.ApplicationUserId == claim.Value, icludeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCardViewModel.OrderHeader.ApplicationUser = unitOfWork.ApplicationUser.GetFirstOreDefault(x => x.Id == claim.Value);
            ShoppingCardViewModel.OrderHeader.Name = ShoppingCardViewModel.OrderHeader.ApplicationUser.Name;
            ShoppingCardViewModel.OrderHeader.PhoneNumber = ShoppingCardViewModel.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCardViewModel.OrderHeader.StreetAddress = ShoppingCardViewModel.OrderHeader.ApplicationUser.StreetHouseAdress;
            ShoppingCardViewModel.OrderHeader.City = ShoppingCardViewModel.OrderHeader.ApplicationUser.City;
            ShoppingCardViewModel.OrderHeader.State = ShoppingCardViewModel.OrderHeader.ApplicationUser.State;
            ShoppingCardViewModel.OrderHeader.PostalCode = ShoppingCardViewModel.OrderHeader.ApplicationUser.PostCode;

            foreach (var cart in ShoppingCardViewModel.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                ShoppingCardViewModel.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }
            return View(ShoppingCardViewModel);

        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPOST(ShoppingCardViewModel ShoppingCardViewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity; // de id van de ingelogde user
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCardViewModel.ListCart = unitOfWork.ShoppingCard.GetAll(u => u.ApplicationUserId == claim.Value, icludeProperties: "Product");
            ShoppingCardViewModel.OrderHeader.PaymentStatus = SD.PaymentStatusPendig ;
            ShoppingCardViewModel.OrderHeader.OrderStatus = SD.StatusPendig;
            ShoppingCardViewModel.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCardViewModel.OrderHeader.ApplicationUserId = claim.Value;
            foreach (var cart in ShoppingCardViewModel.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                ShoppingCardViewModel.OrderHeader.OrderTotal += cart.Price * cart.Count;
            }
            unitOfWork.OrderHeader.Add(ShoppingCardViewModel.OrderHeader);
            unitOfWork.Save();


            foreach (var cart in ShoppingCardViewModel.ListCart)
            {
                OrderDetailsModel orderDetails = new()
                {
                    ProductId = cart.ProductId,
                    OrderId = ShoppingCardViewModel.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                unitOfWork.OrderDetails.Add(orderDetails);
                unitOfWork.Save();
            }

            unitOfWork.ShoppingCard.RemoveRange(ShoppingCardViewModel.ListCart);
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Plus (int cartId)
        {
            var shoppingCard = unitOfWork.ShoppingCard.GetFirstOreDefault(x => x.Id == cartId);
            unitOfWork.ShoppingCard.IncrementCount(shoppingCard, 1);
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var shoppingCard = unitOfWork.ShoppingCard.GetFirstOreDefault(x => x.Id == cartId);
            unitOfWork.ShoppingCard.DecrementCount(shoppingCard, 1);
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove (int cartId)
        {
            var shoppingCard = unitOfWork.ShoppingCard.GetFirstOreDefault(x => x.Id == cartId);
            unitOfWork.ShoppingCard.Remove(shoppingCard);
            unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private double GetPriceBasedOnQuantity (double quantity, double price , double price50, double price100)
        {
            if(quantity <= 50)
            {
                return price;
            }
            else
            {
                if(quantity <= 100)
                {
                    return price50;
                }

                return price100;
            }
        }
    }
}
