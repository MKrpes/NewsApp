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
           
                article.DateTime = DateTime.Now;
                article.PID = null;
                
                if (HttpContext.Session.GetInt32("UserId")!=null)
                {
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
                    using (var memoryStream = new MemoryStream())
                    {
                        photo.CopyTo(memoryStream);
                        var photoEntity = new Photo { ImageData = memoryStream.ToArray() };
                        _context.Photos.Add(photoEntity);
                        _context.SaveChanges();
                        // Set the generated PID (Photo ID) in the article
                        article.PID = photoEntity.PID;
                    }
                }
                // Add the article to the database
                _context.Articles.Add(article);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index)); // Redirect to the article index or another appropriate action
            
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
        public IActionResult Edit(int? id)
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
        public IActionResult Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var article = _context.Articles.Find(id);

            if (article == null)
            {
                return NotFound();
            }
        

            _context.Articles.Remove(article);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index)); // Redirect to the article index or another appropriate action
        }
    }
}