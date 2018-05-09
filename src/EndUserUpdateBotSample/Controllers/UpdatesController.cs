using System.Threading.Tasks;
using System.Web.Mvc;
using EndUserUpdateBotSample.Models;

namespace EndUserUpdateBotSample.Controllers
{
    public class UpdatesController : Controller
    {
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
        public async Task<ActionResult> Create([Bind(Include = "Message")] Update update)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "Registrations");
            }

            return View(update);
        }
    }
}
