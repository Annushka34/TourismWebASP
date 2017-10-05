using AdminWebSite.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using AdminWebSite.Models;
using AdminWebSite.DAL.Entities;

namespace AdminWebSite.Controllers
{
    public class CityController : Controller
    {
        EFContext _context;
        public CityController()
        {
            _context = new EFContext();
            ViewBag.MenuCity = true;
            ViewBag.DelCity = false;
        }
        // GET: City
        public ActionResult Index()
        {
            var model = _context.Cities.Include(c => c.Country)
                .Select(c => new CityViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Priority = c.Priority,
                    DateCreate = c.DateCreate,
                    Country = c.Country.Name
                });
            return View(model);
        }
        public ActionResult Create()
        {
                CityCreateViewModel model = new CityCreateViewModel();
                model.Countries = _context.Countries.Select(x => new SelectItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
                return View(model);
        }
        [HttpPost]
        public ActionResult Create(CityCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Country isFindCountry = _context.Countries.Include(c=>c.Cities).SingleOrDefault(x => x.Id == model.CountryId);
                if(isFindCountry != null)
                {
                    var cities = isFindCountry.Cities;
                    City isFindCity = cities.SingleOrDefault(x => x.Name == model.Name);
                    if (isFindCity != null)
                    {
                        ModelState.AddModelError("", "Нажаль таке місто вже існує - можливо ви помилились точкою на карті)....");
                        model.Countries = _context.Countries.Select(x => new SelectItemViewModel
                        {
                            Id = x.Id,
                            Name = x.Name
                        }).ToList();
                        return View(model);
                    }
                }
                City city = new City
                {
                    Name = model.Name,
                    DateCreate = DateTime.Now,
                    Priority = model.Priority,
                    CountryId = model.CountryId
                };
                _context.Cities.Add(city);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "З містами теж якась лажа....");
                //return RedirectToAction("Create");
                model = new CityCreateViewModel();
                model.Countries = _context.Countries.Select(x => new SelectItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
                return View(model);
            }
        }
        public ActionResult Edit(int id)
        {
            var city = _context.Cities.SingleOrDefault(x => x.Id == id);
            CityEditViewModel model = new CityEditViewModel();
            model.Id = id;
            model.Name = city.Name;
            model.Priority = city.Priority;
            model.CountryId = city.CountryId;
            model.Countries = _context.Countries.Select(x => new SelectItemViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(CityEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                City searchCity = _context.Cities.SingleOrDefault(x => x.Id == model.Id);
                if (searchCity != null)
                {
                    //old.DateCreate = DateTime.Now;
                    searchCity.Name = model.Name;
                    searchCity.Priority = model.Priority;
                    searchCity.CountryId = model.CountryId;
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "І тут затупив:-)))");
                model = new CityEditViewModel();
                model.Countries = _context.Countries.Select(x => new SelectItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
                return View(model);
            }
        }
        public ActionResult Delete(int id)
        {
            var city = _context.Cities.SingleOrDefault(x => x.Id == id);
            if (city != null)
            {
                SelectItemViewModel model = new SelectItemViewModel();
                model.Id = id;
                model.Name = city.Name;
                return View(model);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Delete(SelectItemViewModel model)
        {
            var city = _context.Cities.SingleOrDefault(x => x.Id == model.Id);
            _context.Cities.Remove(city);
            _context.SaveChanges();
            ViewBag.DelCity = true;
            return View(model);
        }
    }
}