using BulkyBook.DataAcces.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> coverTypes = _unitOfWork.CoverType.GetAll();
            return View(coverTypes);
        }

        public IActionResult Create ()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create (CoverType obj)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(obj);
                _unitOfWork.Save();
                TempData["succes"] = "Cover Type Created";
                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }

        }
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var deletPage = _unitOfWork.CoverType.GetFirstOreDefault(x => x.Id == id);
            if (deletPage == null)
            {
                return NotFound();
            }
            return View(deletPage);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteItem(int? id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOreDefault(x => x.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.CoverType.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        { 
            
            if(id== null || id == 0)
            {
                return NotFound();
            }
            var obj = _unitOfWork.CoverType.GetFirstOreDefault(x => x.Id == id);
            if(obj== null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            if (obj == null)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
