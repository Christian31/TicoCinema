using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TicoCinema.WebApplication.Models;
using TicoCinema.WebApplication.Utils;
using TicoCinema.WebApplication.ViewModels;

namespace TicoCinema.WebApplication.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private Entities db = new Entities();

        // GET: Movies
        public ActionResult Index()
        {
            var movies = db.Movie.Include(m => m.AudienceClassification).ToList();
            var moviesViewModels = ConvertMoviesToViewModels(movies);

            return View(moviesViewModels);
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            MovieViewModel movieViewModel = ConvertMovieToViewModel(movie);
            movieViewModel.ImagePath = FileManager.GetMovieImagePath(movieViewModel.ImagePath);
            movieViewModel.Categories = GetCategoriesViewModels(movie.CategoriesAssigned);

            return View(movieViewModel);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            var model = new MovieViewModel()
            {
                Categories = GetCategoriesViewModels(0),
                ReleaseDate = DateTime.Now.AddDays(1)
            };
            ViewBag.AudienceClassificationId = new SelectList(db.AudienceClassification, "AudienceClassificationId", "Name");
            return View(model);
        }

        private IList<SelectListItem> GetCategoriesViewModels(long categoriesSelected)
        {
            List<SelectListItem> categoriesViewModels = new List<SelectListItem>();
            var movieCategories = db.MovieCategory.ToList();
            foreach (var item in movieCategories)
            {
                categoriesViewModels.Add(new SelectListItem()
                {
                    Text = item.CategoryName,
                    Value = item.CategoryId.ToString(),
                    Selected = BitManager.ContainsBit(categoriesSelected, item.BitAssigned)
                });
            }

            return categoriesViewModels;
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MovieViewModel movie)
        {
            if (movie.UploadedFile == null)
            {
                ModelState.AddModelError("UploadedFile", "El campo Imagen es requerido.");
            }
            else if (!FileManager.FileHasValidTypeForImages(movie.UploadedFile))
            {
                ModelState.AddModelError("UploadedFile", "El campo Imagen permite archivos únicamente con formato JPG y PNG.");
            }

            if (ModelState.IsValid)
            {
                var categoriesSelected = db.MovieCategory.
                    Where(item => movie.SelectedCategories.Contains(item.CategoryId.ToString())).
                    Select(item => item.BitAssigned).ToList();

                movie.CategoriesAssigned = BitManager.GetBitsSum(categoriesSelected);
                movie.ImagePath = FileManager.SaveMovieImage(movie.Name, movie.UploadedFile);
                Movie moviedb = ConvertViewModelToMovie(movie);
                db.Movie.Add(moviedb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AudienceClassificationId = new SelectList(db.AudienceClassification, "AudienceClassificationId", "Name", movie.AudienceClassificationId);
            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            MovieViewModel movieViewModel = ConvertMovieToViewModel(movie);
            movieViewModel.Categories = GetCategoriesViewModels(movie.CategoriesAssigned);
            ViewBag.AudienceClassificationId = new SelectList(db.AudienceClassification, "AudienceClassificationId", "Name", movie.AudienceClassificationId);
            return View(movieViewModel);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MovieViewModel movie)
        {
            if (movie.UploadedFile != null && !FileManager.FileHasValidTypeForImages(movie.UploadedFile))
            {
                ModelState.AddModelError("UploadedFile", "El campo Imagen permite archivos únicamente con formato JPG y PNG.");
            }

            if (ModelState.IsValid)
            {
                if (movie.UploadedFile != null)
                {
                    movie.ImagePath = FileManager.ReplaceMovieImage(movie.Name, movie.UploadedFile, movie.ImagePath);
                }

                var categoriesSelected = db.MovieCategory.
                    Where(item => movie.SelectedCategories.Contains(item.CategoryId.ToString())).
                    Select(item => item.BitAssigned).ToList();

                movie.CategoriesAssigned = BitManager.GetBitsSum(categoriesSelected);
                Movie moviedb = ConvertViewModelToMovie(movie);

                db.Entry(moviedb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AudienceClassificationId = new SelectList(db.AudienceClassification, "AudienceClassificationId", "Name", movie.AudienceClassificationId);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            movie.ImagePath = FileManager.GetMovieImagePath(movie.ImagePath);
            MovieViewModel movieViewModel = ConvertMovieToViewModel(movie);
            movieViewModel.Categories = GetCategoriesViewModels(movie.CategoriesAssigned);

            return View(movieViewModel);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movie.Find(id);
            db.Movie.Remove(movie);
            db.CinemaSchedule.RemoveRange(movie.CinemaSchedule);
            db.SaveChanges();

            FileManager.DeleteMovieImage(movie.ImagePath);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<MovieViewModel> ConvertMoviesToViewModels(List<Movie> movies)
        {
            List<MovieViewModel> moviesViewModels = new List<MovieViewModel>();
            foreach (var movie in movies)
            {
                moviesViewModels.Add(ConvertMovieToViewModel(movie));
            }

            return moviesViewModels;
        }

        private Movie ConvertViewModelToMovie(MovieViewModel movieViewModel)
        {
            Movie movie = db.Movie.Find(movieViewModel.MovieId);
            if (movie == null)
            {
                movie = new Movie
                {
                    AudienceClassificationId = movieViewModel.AudienceClassificationId,
                    DurationTime = new TimeSpan(0, int.Parse(movieViewModel.DurationTime), 0),
                    MovieId = movieViewModel.MovieId,
                    Name = movieViewModel.Name,
                    ImagePath = movieViewModel.ImagePath,
                    ReleaseDate = movieViewModel.ReleaseDate,
                    CategoriesAssigned = movieViewModel.CategoriesAssigned
                };
            }
            else
            {
                movie.AudienceClassificationId = movieViewModel.AudienceClassificationId;
                movie.DurationTime = new TimeSpan(0, int.Parse(movieViewModel.DurationTime), 0);
                movie.MovieId = movieViewModel.MovieId;
                movie.Name = movieViewModel.Name;
                movie.ImagePath = movieViewModel.ImagePath;
                movie.ReleaseDate = movieViewModel.ReleaseDate;
                movie.CategoriesAssigned = movieViewModel.CategoriesAssigned;
            }

            return movie;
        }

        private MovieViewModel ConvertMovieToViewModel(Movie movie)
        {
            return new MovieViewModel
            {
                AudienceClassificationId = movie.AudienceClassificationId,
                AudienceClassificationName = movie.AudienceClassification.Name,
                DurationTime = movie.DurationTime.TotalMinutes.ToString(),
                MovieId = movie.MovieId,
                Name = movie.Name,
                ImagePath = movie.ImagePath,
                ReleaseDate = movie.ReleaseDate
            };
        }
    }
}
