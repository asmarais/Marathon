using L_MOBILE_MARATHON.Data;
using L_MOBILE_MARATHON.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace L_MOBILE_MARATHON.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _db;
		public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
		{
			_logger = logger;
			_db = db;
		}

		public IActionResult Index()
		{
            List<Event> objEvents = _db.Events.ToList();

            foreach (var obj in  objEvents)
			{
				if (obj.Status == "open" && obj.Start < DateTime.Today)
				{
					obj.Status = "closed";
                    _db.Events.Update(obj);
                }
			}
            objEvents = _db.Events.OrderBy(e => e.Start).Where(e => e.Status == "open").Take(5).ToList();

            int count_events = _db.Events.Count();
             int count_participant = _db.Participants.Count();

            var myDict = new Dictionary<String, int>
            {
				{ "participants" , count_participant },
				{ "events" , count_events }
            };
            ViewBag.Message = myDict;
            return View(objEvents);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
