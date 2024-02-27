using L_MOBILE_MARATHON.Data;
using L_MOBILE_MARATHON.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using System.Reflection;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace L_MOBILE_MARATHON.Controllers
{
	public class EventController : Controller
	{
		private readonly ApplicationDbContext _db;
		public EventController(ApplicationDbContext db)
		{
			_db = db;
		}
		// GET: EventsController
		public IActionResult Index(string? eventNameFilter = "", string? statusFilter = "", DateTime? startDateFilter = null, int pg=1)
		{
			IQueryable<Event> objEvents = _db.Events.AsQueryable();
			foreach(var obj in objEvents)
			{
                if (obj.Status == "open" && obj.Start < DateTime.Today)
                {
                    obj.Status = "closed";
                    _db.Events.Update(obj);
                }
            }
            _db.SaveChanges();


            //Filter;
            if (!string.IsNullOrEmpty(eventNameFilter))
			{
				objEvents = objEvents.Where(e => e.EventName.ToLower().Contains(eventNameFilter.ToLower()));
			}
			if (!string.IsNullOrEmpty(statusFilter))
			{
				objEvents = objEvents.Where(e => e.Status.ToLower() == statusFilter.ToLower());
			}

			if (startDateFilter.HasValue)
			{
				objEvents = objEvents.Where(e => e.Start.Date == startDateFilter.Value.Date);
			}

			const int pageSize = 10;
			// This not required but if the user trys to send the wrong parameters to the application 
			// This line will prevent this 
			if (pg < 1)
				pg = 1;
			int recsCount = objEvents.Count();
			var pager = new Pager(recsCount, pg, pageSize);
			int recSkip = (pg - 1) * pageSize;
			var data = objEvents.Skip(recSkip).Take(pager.PageSize).ToList();
			this.ViewBag.Pager = pager;
			/*
			if (!string.IsNullOrEmpty(eventNameFilter))
			{
				this.ViewBag.NameFilter = eventNameFilter;
				objEvents  = objEvents.Where(e => e.EventName.ToLower().Contains(eventNameFilter.ToLower())).ToList();
			}*/
			return View(data);
		}
		// Create an event
		public IActionResult Create(EventViewModel obj)
		{
			return View(obj);
		}
		[HttpPost]
		public IActionResult Create(EventViewModel obj, string[] EventType)
		{
			if (obj.Event.Start > obj.Event.End)
			{
				TempData["Error"] = "Start Date must less than End Date";
				return View(obj);
			}
			/*
			if (EventType == null)
			{
				TempData["Error"] = "You should choose at least one Event type";
				return View(obj);
			}
			*/
			if (ModelState.IsValid)
			{
				_db.Events.Add(obj.Event);
				_db.SaveChanges();

				foreach (var eventType in EventType)
				{
					var eventAttribute = new EventAttribute
					{
						Type = eventType,
						EventFK = obj.Event.Id
					};
					_db.EventAttributes.Add(eventAttribute);
				}

				_db.SaveChanges();
				TempData["Success"] = "Event is created successfully";
				return RedirectToAction("Index");
			}

			return View();
		}

		// Update an Event
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			var eventToEdit = _db.Events.Find(id);
			if (eventToEdit == null)
			{
				return NotFound();
			}

			var eventAttributes = _db.EventAttributes.Where(ea => ea.EventFK == id).ToList();

			// Create a dictionary to hold checkbox states

			var eventViewModel = new EventViewModel
			{
				Event = eventToEdit,
				EventAttribute = eventAttributes
			};

			return View(eventViewModel);
		}

		[HttpPost]
		public IActionResult Edit(EventViewModel obj)
		{
			//check this function
			if (ModelState.IsValid)
			{
				_db.Events.Update(obj.Event);

				int id = obj.Event.Id;
		
				_db.SaveChanges(); 
				TempData["Success"] = "Event updated successfully";
				return RedirectToAction("Index");
			}
			return View();
		}
		
		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			Event? eventObj = _db.Events.Find(id);

			if (eventObj == null)
			{
				return NotFound();
			}
			var eventAttributes = _db.EventAttributes.Where(ea => ea.EventFK == id).ToList(); ;
			// Remove the retrieved EventAttributes
			_db.EventAttributes.RemoveRange(eventAttributes);
			// Save changes to the database
			_db.SaveChanges();
			_db.Events.Remove(eventObj);
			_db.SaveChanges();
			TempData["Success"] = "Event deleted succefully";
			return RedirectToAction("Index");
		}
	}
}
