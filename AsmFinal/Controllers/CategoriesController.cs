﻿using AsmFinal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AsmFinal.Controllers
{
    [Authorize(Roles = "Staff")]
    public class CategoriesController : Controller

    {
        private ApplicationDbContext _context;
        public CategoriesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Categories
        public ActionResult Index()
        {
            var list = _context.Categories.ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {

            bool IsProductNameExist = _context.Categories.Any
                 (x => x.Name == category.Name && x.Id != category.Id);
            if (IsProductNameExist == true)
            {
                ModelState.AddModelError("Name", "Category Name already exists");
            }

            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }
        public JsonResult IsProductNameExist(string ProductName, int? Id)
        {
            var validateName = _context.Categories.FirstOrDefault
                                (x => x.Name == ProductName && x.Id != Id);
            if (validateName != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var productInDb = _context.Categories.SingleOrDefault(p => p.Id == id);

            if (productInDb == null)
            {
                return HttpNotFound();
            }

            _context.Categories.Remove(productInDb);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {


            Category category = _context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")]
                          Category category)
        {
            bool IsProductNameExist = _context.Categories.Any
                            (x => x.Name == category.Name && x.Id != category.Id);
            if (IsProductNameExist == true)
            {
                ModelState.AddModelError("Name", "Category Name already exists");
            }

            if (ModelState.IsValid)
            {
                _context.Entry(category).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
    }
}
