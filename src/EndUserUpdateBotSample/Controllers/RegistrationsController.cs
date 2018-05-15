using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EndUserUpdateBotSample.Models;
using EndUserUpdateBotSample.Repositories;

namespace EndUserUpdateBotSample.Controllers
{
    public class RegistrationsController : Controller
    {
        private readonly IRegistrationRepository _repository;
        private Random random = new Random();

        public RegistrationsController(IRegistrationRepository repository)
        {
            _repository = repository;
        }


        // GET: Registrations
        public async Task<ActionResult> Index()
        {
            var registrations = await _repository.GetAll();

            return View(registrations);
        }

        // GET: Registrations/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var registration = await _repository.GetById(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // GET: Registrations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Registrations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PhoneNumber, Name")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                registration.SecurityCode = random.Next(99999).ToString("D5");
                registration.Status = "Unconfirmed";
                await _repository.AddAsync(registration);
                return RedirectToAction("Index");
            }

            return View(registration);
        }

        // GET: Registrations/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var registration = await _repository.GetById(id);
            if (registration== null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Registrations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PhoneNumber, Name, Id")] Registration registration)
        {
            if (ModelState.IsValid)
            {
                var previousRegistration = await _repository.GetById(registration.Id);
                if (previousRegistration == null)
                {
                    return HttpNotFound();
                }

                if(previousRegistration.PhoneNumber != registration.PhoneNumber)
                {
                    registration.Status = "Unconfirmed";
                }
                await _repository.UpdateAsync(registration);
                return RedirectToAction("Index");
            }
            return View(registration);
        }

        // GET: Registrations/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var registration = await _repository.GetById(id);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            await _repository.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
