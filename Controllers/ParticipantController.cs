using L_MOBILE_MARATHON.Data;
using L_MOBILE_MARATHON.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace L_MOBILE_MARATHON.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ParticipantController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: ParticipantController
        public ActionResult Index(string? FirstNameFilter = "", string? SecondNameFilter = "", DateOnly? BirthFilter = null, int pg=1)
        {
			List<Participant> objParticipants = _db.Participants.ToList();

			if (!string.IsNullOrEmpty(FirstNameFilter))
			{
				objParticipants = (List<Participant>)objParticipants.Where(e => e.FirstName.ToLower().Contains(FirstNameFilter.ToLower()));
			}
			if (!string.IsNullOrEmpty(SecondNameFilter))
			{
				objParticipants = (List<Participant>)objParticipants.Where(e => e.SecondName.ToLower().Contains(SecondNameFilter.ToLower()));
			}
			if (BirthFilter.HasValue)
			{
				objParticipants = (List<Participant>)objParticipants.Where(e => e.Birthday == BirthFilter.Value);
			}
			//For Pagination
			const int pageSize = 10;
			
			if (pg < 1)
				pg = 1;
			int recsCount = objParticipants.Count();
			var pager = new Pager(recsCount, pg, pageSize);
			int recSkip = (pg - 1) * pageSize;
			var data = objParticipants.Skip(recSkip).Take(pager.PageSize).ToList();
			this.ViewBag.Pager = pager;
			return View(data);
        }
        // Create a Participant
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Participant obj)
        {
            if (ModelState.IsValid)
            {
                _db.Participants.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Participant is created succefully";
                return RedirectToAction("Index");
            }
            return View();
        }
        // Update a Participant
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Participant? user = _db.Participants.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult Edit(Participant obj)
        {
            if (ModelState.IsValid)
            {
                _db.Participants.Update(obj);
                _db.SaveChanges();
                TempData["Success"] = "Participant updated succefully";

                return RedirectToAction("Index");
            }
            return View();
        }
        
        // Delete an Event
        public IActionResult Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Participant? obj = _db.Participants.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            Participant? obj = _db.Participants.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Participants.Remove(obj);
            _db.SaveChanges();
            TempData["Success"] = "Participant deleted succefully";

            return RedirectToAction("Index");
        }

    }
}
