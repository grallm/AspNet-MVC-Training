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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AspNet_MVC_Training.Controllers
{
    public class TrainingsController : Controller
    {
        private readonly IdentityDataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<TrainingsController> _logger;

        public TrainingsController(IdentityDataContext context, UserManager<ApplicationUser> userManager, ILogger<TrainingsController> logger)
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

            // Populate with Formers
            trainings = trainings.Include(t => t.Former);
            
            // Get User's registered formations
            List<int> registeredFormations = new List<int>();

            if (User.Identity.IsAuthenticated) {
              ApplicationUser userReq = await _userManager.GetUserAsync(User);
              // Populate UserTraining
              ApplicationUser user = await _userManager.Users
                .Include(u => u.UserTrainings)
                .SingleAsync(u => u.Equals(userReq));

              if (user != null) {
                registeredFormations = user.UserTrainings.Select(ut => ut.TrainingID).ToList();
              }
            }

            var trainingCategoryVM = new TrainingCategoryViewModel
            {
                Categories = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                Trainings = await trainings.ToListAsync(),
                UserFormations = registeredFormations
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

            ApplicationUser userReq = await _userManager.GetUserAsync(User);
            ApplicationUser user = await _userManager.Users
              .Include(u => u.UserTrainings)
              .SingleAsync(u => u.Equals(userReq));
            
            if (user.UserTrainings.FirstOrDefault(ut => ut.TrainingID == id) == null) {
              return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Create(string Title, IFormFile Image, string Category, decimal Price, string Description, string Content, string Video)
        // public async Task<IActionResult> Create([Bind("TrainingID,Title,Image,ReleaseDate,Category,Price")] Training training)
        {
            // Allow only logged in user to create
            if (!User.Identity.IsAuthenticated) {
                return View();
            }

            Training training = new Training {
              Title = Title,
              Description = Description,
              Content = Content,
              Video = Video,
              ReleaseDate = DateTime.Now,
              Category = Category,
              Price = Price,
              Former = await _userManager.GetUserAsync(User)
            };

            if (Image != null && Image.Length > 0)  
            {
              var fileName = Path.GetFileName(Image.FileName);
              var fileUrl = Path.Combine("images", Guid.NewGuid().ToString() + "_" + fileName);
              var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileUrl);
              using (var fileStream = new FileStream(filePath, FileMode.Create))
              {
                  await Image.CopyToAsync(fileStream);
              }

              training.Image = fileUrl;
            }  

            UserTraining userTraining = new UserTraining {
              User = training.Former,
              Training = training
            };

            if (TryValidateModel(training, nameof(Training)))
            {
                _context.Add(training);
                _context.Add(userTraining);
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
        public async Task<IActionResult> Register(int TrainingID)
        {
            var training = await _context.Training.FindAsync(TrainingID);
            
            if (training == null) {
              return RedirectToAction(nameof(Index));
            }

            ApplicationUser user = await _userManager.GetUserAsync(User);

            UserTraining userTraining = new UserTraining {
              UserId = user.Id,
              TrainingID = training.TrainingID
            };
            _context.Add(userTraining);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
