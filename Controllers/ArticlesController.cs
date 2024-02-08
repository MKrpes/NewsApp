using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsApp.Data;
using NewsApp.Models;

//type2
namespace NewsApp.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly AppDbContext _context;

        public ArticlesController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Articles.Include(a => a.Photo).Include(a => a.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Article article, IFormFile photo)
        {
            Console.WriteLine("jel se kaj dogadja?");
            //if (ModelState.IsValid)
            {
                Console.WriteLine("jel model state vaild?");
                article.DateTime = DateTime.Now;
                article.PID = null;
                
                if (HttpContext.Session.GetInt32("UserId")!=null)
                {
                    Console.WriteLine("jel se dobiva UID?");
                    var userId = (int)HttpContext.Session.GetInt32("UserId");
                    article.UID = userId;
                }
                else
                {
                    // Handle the case where UID is not available from the session
                    return RedirectToAction("Login", "Account"); // Redirect to login page or another appropriate action
                }

                // Process photo upload
                if (photo != null && photo.Length > 0)
                {
                    Console.WriteLine("jel dobil sliku?");
                    using (var memoryStream = new MemoryStream())
                    {
                        photo.CopyTo(memoryStream);
                        var photoEntity = new Photo { ImageData = memoryStream.ToArray() };
                        _context.Photos.Add(photoEntity);
                        _context.SaveChanges();
                        Console.WriteLine("jel uploadal sliku?");
                        // Set the generated PID (Photo ID) in the article
                        article.PID = photoEntity.PID;
                    }
                }
                Console.WriteLine("jel ide u _context?");
                // Add the article to the database
                _context.Articles.Add(article);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index)); // Redirect to the article index or another appropriate action
            }
            Console.WriteLine("return View(article)");
            return View(article);
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = _context.Articles
                .Include(a => a.User)
                .Include(a => a.Photo)
                .FirstOrDefault(a => a.ID == id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }
    }
}


/*
namespace NewsApp.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly AppDbContext _context;

        public ArticlesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Articles.Include(a => a.Photo).Include(a => a.User);
            return View(await appDbContext.ToListAsync());
        }
        //public async Task<IActionResult> Index()
        //{
        //    var appDbContext = _context.Articles.Include(a => a.Photo).Include(a => a.User);
        //    return View(appDbContext);
        //}


        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Photo)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Create

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Title,Photo,Content")]CreateArticleModel cam)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Get user ID from the current session
        //        var userId = (int)HttpContext.Session.GetInt32("UserId");

        //        // Process photo upload
        //        Photo photo = null;
        //        if (cam.Photo != null && cam.Photo.Length > 0)
        //        {
        //            using (var memoryStream = new MemoryStream())
        //            {
        //                cam.Photo.CopyTo(memoryStream);
        //                photo = new Photo { ImageData = memoryStream.ToArray() };
        //                _context.Photos.Add(photo);
        //                _context.SaveChanges();
        //            }
        //        }

        //        // Create the article
        //        var article = new Article
        //        {
        //            Title = cam.Title,
        //            Content = cam.Content,
        //            DateTime = DateTime.Now
        //            UID = userId,
        //            PID = photo?.PID //?? 0  // Set PhotoId to the new photo's ID or 0 if no photo
        //        };

        //        _context.Articles.Add(article);
        //        _context.SaveChanges();

        //        return RedirectToAction("Index", "Home"); // Redirect to the article index or another appropriate action
        //    }

        //    return View();
        //}
        public async Task<IActionResult> Create([Bind("Title,Content,")] Article article)
        {
            if (ModelState.IsValid)
            {
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            article.DateTime = DateTime.Now;
            var userId = (int)HttpContext.Session.GetInt32("UserId");
            article.UID = userId;

            Photo photo = null;
            if (article.Photo != null && article.Photo.ImageData.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    //article.Photo.ImageData.CopyTo(memoryStream);
                    photo = new Photo { ImageData = memoryStream.ToArray() };
                    _context.Photos.Add(photo);
                    _context.SaveChanges();
                }
            }
            return View(article);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Content,DateTime,UID,PID")] Article article)
        {
            if (id != article.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ID))
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
            return View(article);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Photo)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Articles == null)
            {
                return Problem("Entity set 'AppDbContext.Articles'  is null.");
            }
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
          return (_context.Articles?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
*/