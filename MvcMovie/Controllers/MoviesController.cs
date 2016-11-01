using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _environment;

        private async Task<bool> MovieExists(int id)
        {
            return await _context.Movies.AnyAsync(m => m.ID == id);
        }

        public MoviesController(ApplicationDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: /Movies/
        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }
 
        // GET: /Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.SingleOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: /Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Movies/Create
        [HttpPostAttribute]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult>
        Create([BindAttribute("ID,Title,ReleaseDate,Genre,Price")] Movie movie, IFormFile image)
        {
            if (ModelState.IsValid && (image != null) && (image.Length > 0))
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();

                var uploads = Path.Combine(_environment.WebRootPath, "uploads");
                string imageName = movie.ID.ToString() + Path.GetExtension(image.FileName);
                using (var fileStream = new FileStream(Path.Combine(uploads, imageName), FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: /Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id);
        }
 
        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Genre,Price,ReleaseDate,Title")] Movie movie)
        {
            if (id != movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool doesMovieExist = await MovieExists(movie.ID);
                    if (!doesMovieExist)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: /Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id);
        }

        // POST: /Movies/Delete/5
        [HttpPostAttribute, ActionNameAttribute("Delete")]
        [ValidateAntiForgeryTokenAttribute]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(m => m.ID == id);
            _context.Movies.Remove(movie);
            
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}