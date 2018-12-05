using Microsoft.AspNetCore.Mvc;
using AspBlog.Data;
using Microsoft.AspNetCore.Authorization;
using AspBlog.Models.Article;
using Microsoft.AspNetCore.Identity;
using AspBlog.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AspBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> userManager;

        public ArticleController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.db = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult All()
        {
            List<Article> articles = this.db.Articles.Include(a => a.Author).ToList();

            return View(articles);
        }

        [HttpGet]
        public IActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return StatusCode(404);
            }

            var article = this.db.Articles.Include(a => a.Author).FirstOrDefault(a => a.Id == Id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(ArticleCreateFormModel article)
        {
            var authorId = this.userManager.GetUserId(this.User);

            if (ModelState.IsValid)
            {
                this.db.Articles.Add(new Article
                {
                    Title = article.Title,
                    Content = article.Content,
                    AuthorId = authorId
                });

                this.db.SaveChanges();

                return RedirectToAction(nameof(All));
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return StatusCode(404);
            }

            var article = this.db.Articles.Include(a => a.Author).FirstOrDefault(a => a.Id == Id);

            if (article == null)
            {
                return NotFound();
            }

            if (!IsUserAuthorizedToEdit(article))
            {
                return Forbid();
            }

            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        public IActionResult DeleteProcess(int? Id)
        {
            if (Id == null)
            {
                return StatusCode(404);
            }

            var article = this.db.Articles.Include(a => a.Author).First(a => a.Id == Id);

            if (article == null)
            {
                return NotFound();
            }

            if (!IsUserAuthorizedToEdit(article))
            {
                return Forbid();
            }

            this.db.Articles.Remove(article);
            this.db.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return StatusCode(404);
            }

            var article = this.db.Articles.Include(a => a.Author).First(a => a.Id == Id);

            if (article == null)
            {
                return NotFound();
            }

            if (!IsUserAuthorizedToEdit(article))
            {
                return Forbid();
            }

            var model = new ArticleViewModel();
            model.Id = article.Id;
            model.Title = article.Title;
            model.Content = article.Content;

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(ArticleViewModel model)
        {
            var article = this.db.Articles.Include(a => a.Author).FirstOrDefault(a => a.Id == model.Id);

            if (article == null)
            {
                return NotFound();
            }

            if (!IsUserAuthorizedToEdit(article))
            {
                return Forbid();
            }

            article.Title = model.Title;
            article.Content = model.Content;
            this.db.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        public bool IsUserAuthorizedToEdit(Article article)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = article.IsAuthor(this.User.Identity.Name);

            return isAdmin || isAuthor;
        }
    }
}
