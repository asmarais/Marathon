using L_MOBILE_MARATHON.Data;
using L_MOBILE_MARATHON.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace L_MOBILE_MARATHON.Controllers
{
	public class ParticipantrunController : Controller

	{
		private readonly ApplicationDbContext _db;
		public ParticipantrunController(ApplicationDbContext db)
		{
			_db = db;
		}

		// GET: Display Participantsruns 
		public ActionResult Index(string? eventNameFilter = "", string? eventAttributeFilter = "", string? emailFilter = "")
		{
			var query = from r in _db.Participantsruns
						join a in _db.EventAttributes on r.EventAttributeFK equals a.Id
						join p in _db.Participants on r.ParticipantFK equals p.Id
						join e in _db.Events on a.EventFK equals e.Id
						select new
						{
                            Id = r.Id,
							EventName = e.EventName,
							Distance = a.Type,
							Email = p.Email,
							ParticipationDate = e.Start,
							Status = r.Status
						};

			// Apply filters
			if (!string.IsNullOrEmpty(eventNameFilter))
			{
				query = query.Where(e => e.EventName.ToLower().Contains(eventNameFilter.ToLower()));
			}
			if (!string.IsNullOrEmpty(eventAttributeFilter))
			{
				query = query.Where(e => e.Distance.ToLower() == eventAttributeFilter.ToLower());
			}
			if (!string.IsNullOrEmpty(emailFilter))
			{
				query = query.Where(e => e.Email.ToLower() == emailFilter.ToLower());
			}

			var objParticipantsruns = query.ToList();
            ViewBag.Participantsruns = objParticipantsruns;

			return View();
		}

		// Create a participant run
		public IActionResult Create()
        {
            return View();
        } 
        [HttpPost]
        public IActionResult Create(Participantsrun obj)
        {
            if (ModelState.IsValid)
            {
                _db.Participantsruns.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Run is created succefully";
                return RedirectToAction("Index");
            }
            return View();
        }
        // Update a participant run
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Participantsrun? run = _db.Participantsruns.FirstOrDefault(u => u.Id == id);

            if (run == null)
            {
                return NotFound();
            }
            return View(run);
        }
        [HttpPost]
        public IActionResult Edit(Participantsrun obj)
        {
            if (ModelState.IsValid)
            {
                _db.Participantsruns.Update(obj);
                _db.SaveChanges();
                TempData["Success"] = "Run updated succefully";

                return RedirectToAction("Index");
            }
            return View();

        }
        // Delete a participant run
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Participantsrun? categoryFromDb = _db.Participantsruns.Find(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Participantsrun? obj = _db.Participantsruns.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Participantsruns.Remove(obj);
            _db.SaveChanges();
            TempData["Success"] = "Run deleted succefully";

            return RedirectToAction("Index");
        }

    }
}
