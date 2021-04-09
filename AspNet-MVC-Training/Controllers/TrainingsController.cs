using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AspNet_MVC_Training.Models;
using AspNet_MVC_Training.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNet_MVC_Training.Controllers
{
    public class TrainingsController : Controller
    {
        private readonly IdentityDataContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<TrainingsController> _logger;

        public TrainingsController(IdentityDataContext context, UserManager<IdentityUser> userManager, ILogger<TrainingsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Trainings
        public async Task<IActionResult> Index(string trainingCategory, string searchString)
        {
            // Use LINQ to get list of categories.
            IQueryable<string> categoryQuery = from m in _context.Training
                                            orderby m.Category
                                            select m.Category;

            var trainings = from t in _context.Training
                        select t;

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                trainings = trainings.Where(s => s.Title.ToLower().Contains(searchString));
            }

            if (!string.IsNullOrEmpty(trainingCategory))
            {
                trainingCategory = trainingCategory.ToLower();
                trainings = trainings.Where(x => x.Category.ToLower() == trainingCategory);
            }

            var trainingCategoryVM = new TrainingCategoryViewModel
            {
                Categories = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                Trainings = await trainings.ToListAsync()
            };

            return View(trainingCategoryVM);
        }

        // GET: Trainings/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Training
                .Include(t => t.Former)
                .FirstOrDefaultAsync(m => m.TrainingID == id);
            if (training == null)
            {
                return NotFound();
            }

            ViewBag.Title = training.Title;

            return View(training);
        }

        // GET: Trainings/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trainings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(string Title, string Image, string Category, decimal Price, string Description, string Content, string Video)
        // public async Task<IActionResult> Create([Bind("TrainingID,Title,Image,ReleaseDate,Category,Price")] Training training)
        {
            // Allow only logged in user to create
            if (!User.Identity.IsAuthenticated) {
                return View();
            }

            Training training = new Training {
              Title = Title,
              Image = Image,
              Description = Description,
              Content = Content,
              Video = Video,
              ReleaseDate = DateTime.Now,
              Category = Category,
              Price = Price,
              Former = await _userManager.GetUserAsync(User)
            };

            if (TryValidateModel(training, nameof(Training)))
            {
                _context.Add(training);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(training);
        }

        // GET: Trainings/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Training.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }
            return View(training);
        }

        // POST: Trainings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Image,ReleaseDate,Category,Price,Former")] Training training)
        {
            if (id != training.TrainingID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(training);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingExists(training.TrainingID))
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
            return View(training);
        }

        // GET: Trainings/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.Training
                .FirstOrDefaultAsync(m => m.TrainingID == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: Trainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var training = await _context.Training.FindAsync(id);
            _context.Training.Remove(training);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingExists(int id)
        {
            return _context.Training.Any(e => e.TrainingID == id);
        }

        // POST: Trainings/Register/5
        // Register for a training
        [HttpPost, ActionName("Register")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Register(int id)
        {
            var training = await _context.Training.FindAsync(id);
            _context.Training.Remove(training);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
