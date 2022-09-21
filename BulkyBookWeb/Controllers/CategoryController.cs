using BulkyBook.DataAcces;
using BulkyBook.DataAcces.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; // instance maken niet nodig vermist we dit in de service builder hebben gezet!!

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();  // maakt een list van de data uit de Db table categories
            return View(objCategoryList);
        }

        //GET   
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken] // helpt bij cross site request forgery 
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the name");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["succes"] = "Category create successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var category = _db.Categories.Find(id);
            var categoryFromDb = _unitOfWork.Category.GetFirstOreDefault(c => c.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken] // helpt bij cross site request forgery 
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the name");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);//Update de databse met de nieuwe value waarden 
                _unitOfWork.Save(); //dit moet gebeuren voor opslaan in de database
                TempData["succes"] = "Category edit successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var category = _db.Categories.Find(id);
            var categoryFromDb = _unitOfWork.Category.GetFirstOreDefault(c => c.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken] // helpt bij cross site request forgery 
        public IActionResult DeletePost(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOreDefault(c => c.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(obj);//Update de databse met de nieuwe value waarden 
            _unitOfWork.Save(); //dit moet gebeuren voor opslaan in de database
            TempData["succes"] = "Category deleted successfully";
            return RedirectToAction("Index");

        }
    }
}
