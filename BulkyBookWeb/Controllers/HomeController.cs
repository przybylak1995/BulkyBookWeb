using BulkyBook.DataAcces.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Controllers
{


    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger , IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(icludeProperties: "Category,CoverType");
            return View(products);
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

        public IActionResult Details(int productId)
        {

            if(productId == 0)
            {
                return RedirectToAction("Index");
            }
            ShoppingCart cartObj = new ShoppingCart()
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.Product.GetFirstOreDefault(x => x.Id == productId, icludeProperties: "Category,CoverType")
            };
            return View(cartObj);

        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize]
        //public IActionResult Details(ShoppingCart shopping)
        //{
        //    var claimsIdentity = (ClaimsIdentity)User.Identity; // de id van de ingelogde user
        //    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        //    shopping.ApplicationUserId = claim.Value;
        //    _unitOfWork.ShoppingCard.Add(shopping);
        //    _unitOfWork.Save();
        //    return RedirectToAction(nameof(Index));

        //}

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart cart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity; // de id van de ingelogde user
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            cart.ApplicationUserId = claim.Value;


            ShoppingCart cartFromDb = _unitOfWork.ShoppingCard.GetFirstOreDefault(
                u => u.ApplicationUserId == claim.Value && u.ProductId == cart.ProductId
                );

            if(cartFromDb == null)
            {
                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-6G18KKV\\SQLEXPRESS;Database=Bulky;Trusted_Connection=True;"))
                {
                    conn.Open();
                    string query = "insert into ShoppingCart (ProductId , Count , ApplicationUserId) values (@productId, @count, @applicationUser)";
                    SqlCommand comm = new SqlCommand(query, conn);
                    comm.Parameters.AddWithValue("@productId", cart.ProductId);
                    comm.Parameters.AddWithValue("@count", cart.Count);
                    comm.Parameters.AddWithValue("@applicationUser", cart.ApplicationUserId);
                    comm.ExecuteNonQuery();
                }
            }
            else
            {
                _unitOfWork.ShoppingCard.IncrementCount(cartFromDb, cart.Count);
                _unitOfWork.Save();
            }

       

            return RedirectToAction(nameof(Index));
        }
    }
}