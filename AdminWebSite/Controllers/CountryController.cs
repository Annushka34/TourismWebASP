using AdminWebSite.DAL;
using AdminWebSite.DAL.Entities;
using AdminWebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWebSite.Controllers
{
    public class CountryController : Controller
    {
        EFContext _context;
        public CountryController()
        {
            _context = new EFContext();
        }
        // GET: Country
        public ActionResult Index()//прийнято щоб вертав список всього що є
        {
            ViewBag.MenuCountry = true;
            List<CountryViewModel> model;
            model = _context.Countries.Select(c=>new CountryViewModel {
                Id=c.Id,
                Name=c.Name,
                Priority=c.Priority,
                DateCreate=c.DateCreate
            })
            .OrderBy(c=>c.Priority)
            .ToList();
            return View(model);
        }

        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(CountryCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Country isFind = _context.Countries.SingleOrDefault(x => x.Name == model.Name);
                if(isFind!=null)
                {
                    ModelState.AddModelError("", "Дві країни з однаковою назвою...Знову щось пішло не так)))");
                    return View(model);
                }
                Country country = new Country
                {
                    DateCreate = DateTime.Now,
                    Name = model.Name,
                    Priority = model.Priority
                };
                _context.Countries.Add(country);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Дружок не тупи");
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var country = _context.Countries.SingleOrDefault(x => x.Id == id);
            if (country != null)
            {
                CountryEditViewModel model = new CountryEditViewModel();
                model.Id = id;
                model.Name = country.Name;
                model.Priority = country.Priority;
                return View(model);
            }
            return RedirectToAction("Edit");
        }
        [HttpPost]
        public ActionResult Edit(CountryEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Country searchCountry = _context.Countries.SingleOrDefault(x => x.Id == model.Id);
                if (searchCountry != null)
                {
                    //old.DateCreate = DateTime.Now;
                    searchCountry.Name = model.Name;
                    searchCountry.Priority = model.Priority;
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "І тут затупив:-)))");
            return View(model);
        }
        public ActionResult Delete(int id)
        {
            Country country = _context.Countries.SingleOrDefault(x => x.Id == id);
            if(country!=null)
            {                
                SelectItemViewModel model = new SelectItemViewModel();
                model.Id = id;
                model.Name = country.Name;
                return View(model);
            }
            return RedirectToAction("Delete"); 
        }
        [HttpPost]
        public ActionResult Delete(SelectItemViewModel model)
        {
            Country country = _context.Countries.SingleOrDefault(x => x.Id == model.Id);
            if (country != null)
            {
                _context.Countries.Remove(country);
                _context.SaveChanges();
                return View(model);
            }
            return RedirectToAction("Index");
        }
    }
}