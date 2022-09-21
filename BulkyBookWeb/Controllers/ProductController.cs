using BulkyBook.DataAcces.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers
{

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            
            return View();
        }



   
      

        public IActionResult Upsert(int? id)  // Upsert is create and Update in one view
        {

            ProductViewModel productViewModel = new()
            {
                product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };

            if (id== null || id == 0)
            {
                //Create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productViewModel);
            }
            else
            {
                productViewModel.product = _unitOfWork.Product.GetFirstOreDefault(u => u.Id == id);
                return View(productViewModel);

            }
            return View(productViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert( ProductViewModel obj , IFormFile file)
        {
            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName  = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"Images/products");
                    var extension  = Path.GetExtension(file.FileName);

                    if(obj.product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var filestreams  = new FileStream(Path.Combine(uploads,fileName+extension), FileMode.Create))
                    {
                        file.CopyTo(filestreams);
                    }
                    obj.product.ImageUrl = @$"\Images\products\{fileName}{extension}";
                }

                if(obj.product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.product);
                }

                // _unitOfWork.CoverType.Update(obj);
               
                _unitOfWork.Save();
                TempData["succes"] = "Product created succesfully"; 
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(icludeProperties: "Category"); 
             return Json(new { data = productList }); // new {data werkt volgens mij gelijk een dictionary }
          // return Json(productList);
        }


        [HttpDelete]
        public IActionResult Delete (int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOreDefault(x => x.Id == id);
            if (obj == null)
            {
                return Json(new { succes = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { succes = true , message = "Delete succes!!" });
        }

        #endregion

    }


}
