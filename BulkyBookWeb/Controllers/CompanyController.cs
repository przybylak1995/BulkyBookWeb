using BulkyBook.DataAcces.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CompanyController : Controller
    {

        private readonly IUnitOfWork unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new();
            if(id == null|| id ==0)
            {
                return View(company);
            }
            else
            {
                company = unitOfWork.Company.GetFirstOreDefault(x => x.Id == id);
                return View(company);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {

            if(ModelState.IsValid)
            {
                if(company.Id == 0)
                {
                    unitOfWork.Company.Add(company);
                }
                else
                {
                    unitOfWork.Company.Update(company);
                }

                unitOfWork.Save();
                TempData["succes"] = "Company is Upsert";
                return RedirectToAction("Index");
            }
            return View(company);
        }


        #region API CALLS

        public IActionResult GetAll()
        {
            IEnumerable<Company> companies = unitOfWork.Company.GetAll();
            return Json(new {data = companies });
        }

        [HttpDelete]
        public IActionResult Delete(int?id)
        {
            var obj = unitOfWork.Company.GetFirstOreDefault(x => x.Id == id);

            if(obj == null )
            {
                return Json(new { succes = false, message = "Error while deleting" });
            }

            unitOfWork.Company.Remove(obj);
            unitOfWork.Save();
            return Json(new { succes = true, message = "Item is deleted" });


        }
        #endregion
    }
}
