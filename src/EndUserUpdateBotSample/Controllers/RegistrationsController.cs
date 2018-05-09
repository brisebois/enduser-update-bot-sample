using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EndUserUpdateBotSample.Models;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;
using EndUserUpdateBotSample.Repositories;

namespace EndUserUpdateBotSample.Controllers
{
    public class RegistrationsController : Controller
    {
        private const string EndpointUrl = "https://localhost:8081";
        private const string PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private const string DBName = "EndUserUpdateBot";
        private const string CollectionName = "Registrations";
        private readonly IRegistrationRepository _repository;
        private Random random = new Random();

        private DocumentClient client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

        public RegistrationsController(IRegistrationRepository repository)
        {
            _repository = repository;
        }


        // GET: Registrations
        public async Task<ActionResult> Index()
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<Registration> registrationQuery = this.client.CreateDocumentQuery<Registration>(
                UriFactory.CreateDocumentCollectionUri(DBName, CollectionName), queryOptions);

            return View(registrationQuery.ToList());
        }

        // GET: Registrations/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var registration = await client.ReadDocumentAsync<Registration>(UriFactory.CreateDocumentUri(DBName, CollectionName, id));
            if (registration.Document == null)
            {
                return HttpNotFound();
            }
            return View(registration.Document);
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
                await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DBName, CollectionName), registration);
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
            var registration = await client.ReadDocumentAsync<Registration>(UriFactory.CreateDocumentUri(DBName, CollectionName, id));
            if (registration.Document == null)
            {
                return HttpNotFound();
            }
            return View(registration.Document);
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
                var previousRegistration = await client.ReadDocumentAsync<Registration>(UriFactory.CreateDocumentUri(DBName, CollectionName, registration.Id));
                var previousDocument = previousRegistration.Document;
                if (previousDocument == null)
                {
                    return HttpNotFound();
                }

                previousDocument.Name = registration.Name;
                if(previousDocument.PhoneNumber != registration.PhoneNumber)
                {
                    previousDocument.Status = "Unconfirmed";
                }
                previousDocument.PhoneNumber = registration.PhoneNumber;
                await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DBName, CollectionName, registration.Id), previousRegistration.Document);
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
            var registration = await client.ReadDocumentAsync<Registration>(UriFactory.CreateDocumentUri(DBName, CollectionName, id));
            if (registration.Document == null)
            {
                return HttpNotFound();
            }
            return View(registration.Document);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var registration = await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DBName, CollectionName, id));
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                client.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
