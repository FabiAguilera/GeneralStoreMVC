using GeneralStore.MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeneralStore.MVC.Controllers
{
    public class TransactionController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Transaction
        public ActionResult Index()
        {
            //List<Transaction> transactionList = _db.Transactions.ToList();
            //List<Transaction> orderedList = transactionList.OrderBy(trns => trns.TransactionId).ToList();
            var query = _db.Transactions.ToArray();
            return View(query);
        }

        // GET: Transaction/Create
        public ActionResult Create()
        {
            ViewData["Products"] = _db.Products.Select(product => new SelectListItem
            {
                Text = product.Name,
                Value = product.ProductId.ToString()
            }).ToArray();
            ViewData["Customers"] = _db.Customers.AsEnumerable().Select(customer => new SelectListItem
            {
                Text = customer.FullName,
                Value = customer.CustomerId.ToString()
            });
            return View();
        }

        // POST: Create Transaction/Create/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Transaction transaction)
        {
            //if (ModelState.IsValid)
            //{
            //    _db.Transactions.Add(transaction);
            //    _db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            ViewData["Products"] = _db.Products.Select(product => new SelectListItem
            {
                Text = product.Name,
                Value = product.ProductId.ToString()
            }).ToArray();
            ViewData["Customers"] = _db.Customers.AsEnumerable().Select(customer => new SelectListItem
            {
                Text = customer.FullName,
                Value = customer.CustomerId.ToString()
            });
            if (_db.Products.Find(transaction.ProductId) == null)
            {
                ViewData["Error"] = "Invalid Product Id";
                return View(transaction);
            }
            else if (_db.Customers.Find(transaction.CustomerId) == null)
            {
                ViewData["Error"] = "Invalid Customer Id";
                return View(transaction);
            }
            transaction.DateOfTransaction = DateTime.Now;
            _db.Transactions.Add(transaction);
            if (_db.SaveChanges() == 1)
            {
                return Redirect("/Transaction");
            }
            return View(transaction);
        }

        // GET: DELETE Product/Delete/{id}
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: DELETE Product/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Transaction transaction = _db.Transactions.Find(id);
            _db.Transactions.Remove(transaction);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: EDIT Product/Edit/{id}
        public ActionResult Edit(int? id)
        {
            //if (id == null)
            //{
            //    new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            //}
            //Transaction transaction = _db.Transactions.Find(id);

            var entity = _db.Transactions.Find(id);
            if (entity == null)
            {
                ViewData["Error"] = "Invalid Transaction Id";
                return Redirect("/Transaction");
            }
            var customers = _db.Customers.AsEnumerable();
            var products = _db.Products.AsEnumerable();
            ViewData["Customers"] = customers.Select(c => new SelectListItem
            {
                Text = c.FullName,
                Value = c.CustomerId.ToString()
            });
            ViewData["Products"] = products.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.ProductId.ToString()
            });
            return View(entity);
        }

        // POST: EDIT Product/Edit/{id}
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Transaction model)
        {
            //if (ModelState.IsValid)
            //{
            //    _db.Entry(transaction).State = EntityState.Modified;
            //    _db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(transaction);

            var entity = _db.Transactions.Find(id);
            if (entity == null)
            {
                return Redirect("/Transaction");
            }
            entity.CustomerId = model.CustomerId;
            entity.ProductId = model.ProductId;
            if (_db.SaveChanges() == 1)
            {
                return Redirect("/Transaction");
            }
            ViewData["Error"] = "Couldn't update your transaction, please try again.";
            return View(model);
        }

        // GET: DETAILS Product/Details/{id}
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

    }
}