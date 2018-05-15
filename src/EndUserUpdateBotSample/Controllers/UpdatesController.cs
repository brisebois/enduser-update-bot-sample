using System.Threading.Tasks;
using System.Web.Mvc;
using EndUserUpdateBotSample.Models;
using EndUserUpdateBotSample.Repositories;

namespace EndUserUpdateBotSample.Controllers
{
    public class UpdatesController : Controller
    {
        private readonly IRegistrationRepository _repository;
        public UpdatesController(IRegistrationRepository repository)
        {
            _repository = repository;
        }

        // GET: Updates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Updates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Publish([Bind(Include = "Message")] Update update)
        {
            if (ModelState.IsValid)
            {
                var confirmedRegistrations = await _repository.GetByStatus("Confirmed");
            }

            return RedirectToAction("Create");
        }
    }
}
