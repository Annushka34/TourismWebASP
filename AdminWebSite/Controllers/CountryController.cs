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

        public ActionResult Edit(int id)
        {
            var country = _context.Countries.SingleOrDefault(x => x.Id == id);
            CountryEditViewModel model = new CountryEditViewModel();
            model.Id = id;
            model.Name = country.Name;
            model.Priority = country.Priority;
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(CountryEditViewModel model)
        {
            Country searchCountry=_context.Countries.SingleOrDefault(x => x.Id == model.Id);
            if (searchCountry != null)
            {
                //old.DateCreate = DateTime.Now;
                searchCountry.Name = model.Name;
                searchCountry.Priority = model.Priority;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}