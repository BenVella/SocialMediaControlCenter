using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMCC.Models;
using Microsoft.AspNet.Identity;

namespace SMCC.Controllers
{
    public class ArticlesController : Controller
    {
        private Entities db = new Entities();


        public ArticlesController()
        {

        }
        // Get List of relevant articles
        // GET: Articles
        [Authorize]
        public ActionResult Index()
        {
            IEnumerable<Publish> toPublish = db.Publishes.ToList();

            foreach (Publish p in toPublish)
            {
                if (p.PublishDate <= DateTime.Today)
                {
                    // Publish our article
                    p.Article.ArticleStateId = 4;
                    db.Publishes.Remove(p);
                }
            }
            db.SaveChanges();
            return View(db.Articles.ToList());
        }

        // GET: Articles/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Article article = db.Articles.Find(id);

            if (article == null)
            {
                return HttpNotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        [Authorize(Roles = "Writer")]
        public ActionResult Create()
        {
            ViewBag.ArticleUserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Writer")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArticleId,ArticleUserId,Title,Text")] Article article)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.Name != "TestUser")
                    article.ArticleUserId = User.Identity.GetUserId();
                else // Testing
                    article.ArticleUserId = "6cb6eea7-9009-425e-8f29-7d5c90793f77";

                article.DateCreated = DateTime.Today;
                article.ArticleStateId = 1;
                db.Articles.Add(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArticleUserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View(article);
        }

        // GET: Articles/PendingReview
        [Authorize(Roles = "Writer")]
        public ActionResult PendingReview ()
        {
            IEnumerable<Article> articles = db.Articles.Where(a => a.ArticleState.StateName == "Pending");
            return View(articles);
        }

        // GET: Articles/Review
        [Authorize(Roles = "Writer")]
        public ActionResult Review(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Create article
            Article article = db.Articles.Find(id);

            if (article == null)
            {
                return HttpNotFound();
            }

            // Using the View Model created
            var articleReview = new ArticleReview
            {
                articleModel = article, // Use found article attached
                reviewModel = new Review()  // Insert a new empty Review
            };

            return View(articleReview);
        }

        // POST: Articles/Review
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Writer")]
        [ValidateAntiForgeryToken]
        public ActionResult Review(ArticleReview reviewArticle)
        {
            if (ModelState.IsValid)
            {
                // We have a  valid model, let's grab the article from our ViewModel
                Article article = db.Articles.SingleOrDefault(a => a.ArticleId == reviewArticle.articleModel.ArticleId);

                if (reviewArticle.reviewModel.Accepted)
                    article.ArticleStateId = 2; // Set as Reviewed
                else
                    article.ArticleStateId = 3; // Set as Finished

                // Create a new review to add to DB
                Review review = new Review();

                // Pull data from View Model
                review.Text = reviewArticle.reviewModel.Text;
                review.Accepted = reviewArticle.reviewModel.Accepted;
                review.ReviewUserId = User.Identity.GetUserId(); // Use current logged in user's ID
                review.DateReviewed = DateTime.Now;

                db.Reviews.Add(review); // Add new review

                article.ArticleReviewId = review.ReviewId; // Add the new review id created to the article to save.

                db.SaveChanges();       // Commit all changes
                return RedirectToAction("PendingReview");   // Pull back new list
            }

            // Redo process since model was invalid.
            return View(reviewArticle);
        }

        // GET: Articles/PendingReview
        [Authorize(Roles = "MediaManager")]
        public ActionResult PendingEditorial()
        {
            IEnumerable<Article> articles = db.Articles.Where(a => a.ArticleState.StateName == "Reviewed");
            return View(articles);
        }

        // GET: Articles/Review
        [Authorize(Roles = "MediaManager")]
        public ActionResult EditorialReview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Create article
            Article article = db.Articles.Find(id);
            Review review = db.Articles.Find(id).Review;

            if (article == null || review == null)
            {
                return HttpNotFound();
            }

            // Using the View Model created
            var articleReview = new ArticleReview
            {
                articleModel = article, // Use found article attached
                reviewModel = review  // Insert a new empty Review
            };

            return View(articleReview);
        }

        // POST: Articles/Review
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "MediaManager")]
        [ValidateAntiForgeryToken]
        public ActionResult EditorialReview(ArticleReview reviewArticle)
        {
            if (ModelState.IsValid)
            {
                // We have a  valid model, let's grab the article from our ViewModel
                Article article = db.Articles.SingleOrDefault(a => a.ArticleId == reviewArticle.articleModel.ArticleId);
                Review review = db.Reviews.SingleOrDefault(r => r.ReviewId == reviewArticle.reviewModel.ReviewId);

                // Update database items from review model.
                article.Title = reviewArticle.articleModel.Title;
                article.Text = reviewArticle.articleModel.Text;
                review.Text = reviewArticle.reviewModel.Text;
                
                // Go up another state to finished
                article.ArticleStateId = 3;
                // Set The Date Edited for the Review
                review.DateEdited = DateTime.Now;
                
                // See post editorial review if we should publish article
                if (reviewArticle.reviewModel.Accepted && reviewArticle.articleModel.DatePublished != null)
                {
                    review.Accepted = reviewArticle.reviewModel.Accepted;
                    article.DatePublished = reviewArticle.articleModel.DatePublished;
                    Publish publish = new Publish();
                    publish.ArticleId = article.ArticleId;
                    publish.PublishDate = (DateTime)article.DatePublished;
                    db.Publishes.Add(publish);
                }
                // Accepted but no DatePublished included, need to improve.
                else if (reviewArticle.reviewModel.Accepted && reviewArticle.articleModel.DatePublished == null)
                {
                    ViewBag.Message = "If article is accepted, Date Published must be filled in.";

                    reviewArticle.articleModel.AspNetUser = article.AspNetUser;
                    reviewArticle.articleModel.DateCreated = article.DateCreated;
                    return View(reviewArticle);
                }

                db.SaveChanges();       // Commit all changes
                return RedirectToAction("PendingEditorial");   // Pull back new list
            }

            // Redo process since model was invalid.
            return View(reviewArticle);
        }

        [Authorize(Roles = "Writer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            if (article.ArticleState.StateName != "Pending" && article.ArticleState.StateName !=  "Finished" && !article.Review.Accepted)
            {
                article = null;
                ViewBag.Message = "Only Pending / Finished Articles may be updated.";
            }
            
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Writer")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArticleId,ArticleUserId,ArticleReviewId,Title,Text,DateCreated,ArticleStateId")] Article article)
        {
            if (ModelState.IsValid)
            {
                if (article.ArticleStateId == 1 || article.ArticleStateId == 3)
                {
                    if (article.Review != null)
                        article.Review.Accepted = false;
                    article.ArticleStateId = 1; // Revert back to pending if finished

                    db.Entry(article).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else {
                    ViewBag.Message = "Only Pending / Finished Articles may be updated.";
                }
            }
           
            return View(article);
        }

        // GET: Articles/Delete/5
        [Authorize(Roles = "MediaManager,Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = db.Articles.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "MediaManager,Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Get the Article
            Article article = db.Articles.Find(id);
            // If enabled for deletion commit and finish
            if (article.ArticleStateId == 1 || (article.ArticleStateId == 3 && !article.Review.Accepted))
            {
                db.Articles.Remove(article);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "Article Selected may not be deleted.";
                return View(article);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public int ArticleCount()
        {
            return db.Articles.Count();
        }
    }
}
