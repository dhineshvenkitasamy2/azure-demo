using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzureDemo.Models;

namespace AzureDemo.Controllers
{
    public class NotificationModelsController : Controller
    {
        private readonly ConnectIndividualDemoContext _context;

        public NotificationModelsController(ConnectIndividualDemoContext context)
        {
            _context = context;
        }

        // GET: NotificationModels
        public async Task<IActionResult> Index()
        {
              return View(await _context.Notification.ToListAsync());
        }

        // GET: NotificationModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Notification == null)
            {
                return NotFound();
            }

            var notificationModel = await _context.Notification
                .FirstOrDefaultAsync(m => m.id == id);
            if (notificationModel == null)
            {
                return NotFound();
            }

            return View(notificationModel);
        }

        // GET: NotificationModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NotificationModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Content,Tags")] NotificationModel notificationModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(notificationModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notificationModel);
        }

        // GET: NotificationModels/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Notification == null)
            {
                return NotFound();
            }

            var notificationModel = await _context.Notification.FindAsync(id);
            if (notificationModel == null)
            {
                return NotFound();
            }
            return View(notificationModel);
        }

        // POST: NotificationModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("id,Content,Tags")] NotificationModel notificationModel)
        {
            if (id != notificationModel.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notificationModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotificationModelExists(notificationModel.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(notificationModel);
        }

        // GET: NotificationModels/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Notification == null)
            {
                return NotFound();
            }

            var notificationModel = await _context.Notification
                .FirstOrDefaultAsync(m => m.id == id);
            if (notificationModel == null)
            {
                return NotFound();
            }

            return View(notificationModel);
        }

        // POST: NotificationModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Notification == null)
            {
                return Problem("Entity set 'ConnectIndividualDemoContext.Notification'  is null.");
            }
            var notificationModel = await _context.Notification.FindAsync(id);
            if (notificationModel != null)
            {
                _context.Notification.Remove(notificationModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotificationModelExists(string id)
        {
          return _context.Notification.Any(e => e.id == id);
        }
    }
}
