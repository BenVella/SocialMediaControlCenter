using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web;
using System.Security.Principal;
using System.Web.Mvc;
using System.Transactions;
using SMCC.DataAccess;
using System.Data.Entity.Validation;

namespace SMCC.Controllers.Tests
{
    [TestClass()]
    public class ArticlesControllerTests
    {
        ArticlesController articleController;
        ArticlesRepository articleRepository;

        public ArticlesControllerTests()
        {
            articleController = new ArticlesController();
        }

        public bool AssertSameArticles(SMCC.Models.Article originalArticle, SMCC.Models.Article newArticle)
        {
            bool articlesEqual = true;

            if (originalArticle.ArticleId != newArticle.ArticleId) articlesEqual = false;
            if (originalArticle.ArticleUserId != newArticle.ArticleUserId) articlesEqual = false;
            if (originalArticle.ArticleStateId != newArticle.ArticleStateId) articlesEqual = false;
            if (originalArticle.ArticleReviewId != newArticle.ArticleReviewId) articlesEqual = false;
            if (originalArticle.DateCreated != newArticle.DateCreated) articlesEqual = false;
            if (originalArticle.DatePublished != newArticle.DatePublished) articlesEqual = false;
            if (originalArticle.Title != newArticle.Title) articlesEqual = false;
            if (originalArticle.Text != newArticle.Text) articlesEqual = false;

            return articlesEqual;
        }

        public bool AssertModifiedArticles(SMCC.Models.Article originalArticle, SMCC.Models.Article editedArticle)
        {
            bool articlesEqualMod = true;

            if (originalArticle.ArticleId != editedArticle.ArticleId) articlesEqualMod = false; //Id Should remain the same
            if (originalArticle.ArticleUserId != editedArticle.ArticleUserId) articlesEqualMod = false; //UserId Should remain the same
            if (originalArticle.Title == editedArticle.Title) articlesEqualMod = false;  //Title should change if modified correctly
            if (originalArticle.Text == editedArticle.Text) articlesEqualMod = false; //Text should change if modified correctly

            return articlesEqualMod;
        }

        public Mock<ControllerContext> MockContext()
        {
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("TestUser");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

            return controllerContext;
        }
        
        // #### CREATE ####

        [TestMethod()]
        public void CreateArticle_Short_Valid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-creation of new article   

                SMCC.Models.Article newArticle = new SMCC.Models.Article();
                SMCC.Models.Article createdArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try
                {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;

                    newArticle.Title = "A";
                    newArticle.Text = "A";

                    articleController.Create(newArticle);
                }
                finally
                {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();
                    createdArticle = articleRepository.GetArticle(newArticle.ArticleId);

                    Assert.AreEqual(count + 1, existArticles.Count());
                    CollectionAssert.Contains(existArticles, createdArticle);
                    
                }
            }
        }

        [TestMethod()]
        public void CreateArticle_Long_Valid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-creation of new article   

                SMCC.Models.Article newArticle = new SMCC.Models.Article();
                SMCC.Models.Article createdArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try
                {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;
                    
                    for (int i = 0; i < 50; i++)
                    {
                        newArticle.Title += "A";
                    }

                    newArticle.Text = "A";

                    articleController.Create(newArticle);
                }
                finally
                {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();
                    createdArticle = articleRepository.GetArticle(newArticle.ArticleId);

                    Assert.AreEqual(count + 1, existArticles.Count());
                    CollectionAssert.Contains(existArticles, createdArticle);
                    
                }
            }
        }

        [ExpectedException(typeof(DbEntityValidationException))]
        [TestMethod()]
        public void CreateArticle_OutOfRange_Title_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-creation of new article   

                SMCC.Models.Article newArticle = new SMCC.Models.Article();
                SMCC.Models.Article createdArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try
                {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;

                    for (int i = 0; i < 51; i++) {
                        newArticle.Title += "A";
                    }

                    newArticle.Text = "A";
                    articleController.Create(newArticle);
                }
                finally
                {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();
                    createdArticle = articleRepository.GetArticle(newArticle.ArticleId);

                    Assert.AreEqual(count, existArticles.Count());
                    CollectionAssert.DoesNotContain(existArticles, createdArticle);
                    
                }
            }
        }

        [ExpectedException(typeof(DbEntityValidationException))]
        [TestMethod()]
        public void CreateArticle_Empty_Title_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-creation of new article   

                SMCC.Models.Article newArticle = new SMCC.Models.Article();
                SMCC.Models.Article createdArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try
                {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;
                    newArticle.Title = String.Empty;
                    newArticle.Text = "A";

                    articleController.Create(newArticle);
                }
                finally
                {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();
                    createdArticle = articleRepository.GetArticle(newArticle.ArticleId);

                    Assert.AreEqual(count, existArticles.Count());
                    CollectionAssert.DoesNotContain(existArticles, createdArticle);
                    
                }
            }
        }

        [ExpectedException(typeof(DbEntityValidationException))]
        [TestMethod()]
        public void CreateArticle_Empty_Content_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-creation of new article   

                SMCC.Models.Article newArticle = new SMCC.Models.Article();
                SMCC.Models.Article createdArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try
                {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;
                    newArticle.Title = "A";
                    newArticle.Text = String.Empty;

                    articleController.Create(newArticle);
                }
                finally
                {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();
                    createdArticle = articleRepository.GetArticle(newArticle.ArticleId);

                    Assert.AreEqual(count, existArticles.Count());
                    CollectionAssert.DoesNotContain(existArticles, createdArticle);
                    
                }
            }
        }

        [ExpectedException(typeof(DbEntityValidationException))]
        [TestMethod()]
        public void CreateArticle_Null_Title_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-creation of new article   

                SMCC.Models.Article newArticle = new SMCC.Models.Article();
                SMCC.Models.Article createdArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try
                {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;
                    newArticle.Title = null;
                    newArticle.Text = "A";

                    articleController.Create(newArticle);
                }
                finally
                {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();
                    createdArticle = articleRepository.GetArticle(newArticle.ArticleId);

                    Assert.AreEqual(count, existArticles.Count());
                    CollectionAssert.DoesNotContain(existArticles, createdArticle);
                    
                }
            }
        }

        [ExpectedException(typeof(DbEntityValidationException))]
        [TestMethod()]
        public void CreateArticle_Null_Content_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-creation of new article   

                SMCC.Models.Article newArticle = new SMCC.Models.Article();
                SMCC.Models.Article createdArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try
                {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;
                    newArticle.Title = "A";
                    newArticle.Text = null;

                    articleController.Create(newArticle);
                }
                finally
                {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();
                    createdArticle = articleRepository.GetArticle(newArticle.ArticleId);

                    Assert.AreEqual(count, existArticles.Count());
                    CollectionAssert.DoesNotContain(existArticles, createdArticle);
                    
                }
            }
        }

        // #### DETAILS ####

        [TestMethod()]
        public void DetailsArticle_Existing_Id_Valid()
        {
            articleRepository = new ArticlesRepository();
            SMCC.Models.Article expectedArticle = articleRepository.GetArticle(1);

            var result = articleController.Details(1) as ViewResult;
            SMCC.Models.Article actualArticle = (SMCC.Models.Article)result.ViewData.Model;

            Assert.IsTrue(AssertSameArticles(expectedArticle, actualArticle));
        }

        [TestMethod()]
        public void DetailsArticle_NonExisting_Id_Invalid()
        {
            var result = (HttpNotFoundResult)articleController.Details(455);
            Assert.AreEqual(404, result.StatusCode); //404 - Page Not Found        
        }

        [TestMethod()]
        public void DetailsArticle_OutOfBounds_Id_Invalid()
        {
            var result = (HttpNotFoundResult)articleController.Details(0);
            Assert.AreEqual(404, result.StatusCode); //404 - Page Not Found        
        }

        [TestMethod]
        public void DetailsArticle_Negative_Id_Invalid()
        {
            var result = (HttpNotFoundResult)articleController.Details(-1);           
            Assert.AreEqual(404, result.StatusCode); //404 - Page Not Found 
        }

        // #### EDIT ####

        [TestMethod()]
        public void EditArticle_Short_Valid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-editting of existing article   

                SMCC.Models.Article originalArticle = new SMCC.Models.Article();
                SMCC.Models.Article modArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try
                {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;

                    originalArticle = articleRepository.GetArticle(1);

                    var result = articleController.Edit(1) as ViewResult;
                    modArticle = (SMCC.Models.Article)result.ViewData.Model;

                    modArticle.Title = "A";
                    modArticle.Text = "A";

                    articleController.Edit(modArticle);

                }
                finally
                {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();
                    Assert.AreEqual(count, existArticles.Count());  //Record count should remain the same after editting

                    SMCC.Models.Article originalArticle_Modified = new SMCC.Models.Article();
                    originalArticle_Modified = articleRepository.GetArticle(1);
                    Assert.IsFalse(AssertSameArticles(originalArticle, originalArticle_Modified)); //Article content should change, hence return false
                    Assert.IsTrue(AssertModifiedArticles(originalArticle, originalArticle_Modified));

                    
                }
            }
        }

        [TestMethod()]
        public void EditArticle_Long_Valid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-editting of existing article   

                SMCC.Models.Article originalArticle = new SMCC.Models.Article();
                SMCC.Models.Article modArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try
                {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;

                    originalArticle = articleRepository.GetArticle(1);

                    var result = articleController.Edit(1) as ViewResult;
                    modArticle = (SMCC.Models.Article)result.ViewData.Model;

                    modArticle.Title = "";
                    for (int i = 0; i < 50; i++)
                    {
                        modArticle.Title += "A";
                    }
                    modArticle.Text = "A";

                    articleController.Edit(modArticle);

                }
                finally
                {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();
                    Assert.AreEqual(count, existArticles.Count());  //Record count should remain the same after editting

                    SMCC.Models.Article originalArticle_Modified = new SMCC.Models.Article();
                    originalArticle_Modified = articleRepository.GetArticle(1);
                    Assert.IsFalse(AssertSameArticles(originalArticle, originalArticle_Modified)); //Article content should change, hence return false
                    Assert.IsTrue(AssertModifiedArticles(originalArticle, originalArticle_Modified));

                    
                }
            }
        }

        [ExpectedException(typeof(DbEntityValidationException))]
        [TestMethod()]
        public void EditArticle_OutOfRange_Title_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-editting of existing article   

                SMCC.Models.Article originalArticle = new SMCC.Models.Article();
                SMCC.Models.Article modArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;

                    originalArticle = articleRepository.GetArticle(1);

                    var result = articleController.Edit(1) as ViewResult;
                    modArticle = (SMCC.Models.Article)result.ViewData.Model;

                    modArticle.Title = "";
                    for (int i = 0; i < 51; i++) {
                        modArticle.Title += "A";
                    }
                    modArticle.Text = "A";
                    articleController.Edit(modArticle);
                } finally {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();
                    Assert.AreEqual(count, existArticles.Count());  //Record count should remain the same after editing

                    SMCC.Models.Article originalArticle_Modified = new SMCC.Models.Article();
                    originalArticle_Modified = articleRepository.GetArticle(1);
                    Assert.IsTrue(AssertSameArticles(originalArticle, originalArticle_Modified)); //Article content shouldn't change, hence return true
                    Assert.IsFalse(AssertModifiedArticles(originalArticle, originalArticle_Modified));

                    
                }
            }
        }

        [ExpectedException(typeof(DbEntityValidationException))]
        [TestMethod()]
        public void EditArticle_Empty_Title_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-editting of existing article   

                SMCC.Models.Article originalArticle = new SMCC.Models.Article();
                SMCC.Models.Article modArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try
                {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;

                    originalArticle = articleRepository.GetArticle(1);

                    var result = articleController.Edit(1) as ViewResult;
                    modArticle = (SMCC.Models.Article)result.ViewData.Model;

                    modArticle.Title = String.Empty;
                    modArticle.Text = "A";

                    articleController.Edit(modArticle);

                }
                finally
                {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();
                    Assert.AreEqual(count, existArticles.Count());  //Record count should remain the same after editing

                    SMCC.Models.Article originalArticle_Modified = new SMCC.Models.Article();
                    originalArticle_Modified = articleRepository.GetArticle(1);
                    Assert.IsTrue(AssertSameArticles(originalArticle, originalArticle_Modified)); //Article content shouldn't change, hence return true
                    Assert.IsFalse(AssertModifiedArticles(originalArticle, originalArticle_Modified));

                    
                }
            }
        }

        [ExpectedException(typeof(DbEntityValidationException))]
        [TestMethod()]
        public void EditArticle_Empty_Content_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-editting of existing article   

                SMCC.Models.Article originalArticle = new SMCC.Models.Article();
                SMCC.Models.Article modArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try
                {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;

                    originalArticle = articleRepository.GetArticle(1);

                    var result = articleController.Edit(1) as ViewResult;
                    modArticle = (SMCC.Models.Article)result.ViewData.Model;

                    modArticle.Title = "A";
                    modArticle.Text = String.Empty;

                    articleController.Edit(modArticle);

                }
                finally
                {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();
                    Assert.AreEqual(count, existArticles.Count());  //Record count should remain the same after editing

                    SMCC.Models.Article originalArticle_Modified = new SMCC.Models.Article();
                    originalArticle_Modified = articleRepository.GetArticle(1);
                    Assert.IsTrue(AssertSameArticles(originalArticle, originalArticle_Modified)); //Article content shouldn't change, hence return true
                    Assert.IsFalse(AssertModifiedArticles(originalArticle, originalArticle_Modified));

                    
                }
            }
        }

        [ExpectedException(typeof(DbEntityValidationException))]
        [TestMethod()]
        public void EditArticle_Null_Title_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-editting of existing article   

                SMCC.Models.Article originalArticle = new SMCC.Models.Article();
                SMCC.Models.Article modArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try
                {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;

                    originalArticle = articleRepository.GetArticle(1);

                    var result = articleController.Edit(1) as ViewResult;
                    modArticle = (SMCC.Models.Article)result.ViewData.Model;

                    modArticle.Title = null;
                    modArticle.Text = "A";

                    articleController.Edit(modArticle);

                }
                finally
                {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();
                    Assert.AreEqual(count, existArticles.Count());  //Record count should remain the same after editing

                    SMCC.Models.Article originalArticle_Modified = new SMCC.Models.Article();
                    originalArticle_Modified = articleRepository.GetArticle(1);
                    Assert.IsTrue(AssertSameArticles(originalArticle, originalArticle_Modified)); //Article content shouldn't change, hence return true
                    Assert.IsFalse(AssertModifiedArticles(originalArticle, originalArticle_Modified));

                    
                }
            }
        }

        [ExpectedException(typeof(DbEntityValidationException))]
        [TestMethod()]
        public void EditArticle_Null_Content_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-editting of existing article   

                SMCC.Models.Article originalArticle = new SMCC.Models.Article();
                SMCC.Models.Article modArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try
                {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;

                    originalArticle = articleRepository.GetArticle(1);

                    var result = articleController.Edit(1) as ViewResult;
                    modArticle = (SMCC.Models.Article)result.ViewData.Model;

                    modArticle.Title = "A";
                    modArticle.Text = null;

                    articleController.Edit(modArticle);

                }
                finally
                {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();
                    Assert.AreEqual(count, existArticles.Count());  //Record count should remain the same after editing

                    SMCC.Models.Article originalArticle_Modified = new SMCC.Models.Article();
                    originalArticle_Modified = articleRepository.GetArticle(1);
                    Assert.IsTrue(AssertSameArticles(originalArticle, originalArticle_Modified)); //Article content shouldn't change, hence return true
                    Assert.IsFalse(AssertModifiedArticles(originalArticle, originalArticle_Modified));

                    
                }
            }
        }

        [TestMethod()]
        public void EditArticle_NonExisting_Id_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                var result = (HttpNotFoundResult)articleController.Edit(455);
                Assert.AreEqual(404, result.StatusCode); //404 - Page Not Found
            }
        }

        [TestMethod()]
        public void EditArticle_OutOfBounds_Id_Invalid()
        {
            var result = (HttpNotFoundResult)articleController.Edit(0);
            Assert.AreEqual(404, result.StatusCode); //404 - Page Not Found        
        }

        [TestMethod]
        public void EditArticle_Negative_Id_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                var result = (HttpNotFoundResult)articleController.Edit(-1);
                Assert.AreEqual(404, result.StatusCode); //404 - Page Not Found
            }
        }

        // #### DELETE ####

        [TestMethod()]
        public void DeleteArticle_Valid_Id()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-editting of existing article    

                SMCC.Models.Article originalArticle = new SMCC.Models.Article();
                SMCC.Models.Article deleteArticle = new SMCC.Models.Article();
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                try
                {
                    //Set your controller ControllerContext with fake context
                    articleController.ControllerContext = MockContext().Object;

                    originalArticle = articleRepository.GetArticle(1);

                    var result = articleController.Delete(1) as ViewResult;
                    deleteArticle = (SMCC.Models.Article)result.ViewData.Model;

                    articleController.DeleteConfirmed(deleteArticle.ArticleId);

                }
                finally
                {
                    articleRepository = new ArticlesRepository();
                    existArticles = articleRepository.GetAllArticles().ToList();

                    Assert.AreEqual(count - 1, existArticles.Count()); //Record count should be deducted by 1 after deletion
                    CollectionAssert.DoesNotContain(existArticles, originalArticle); // Article should no longer be present in db.
                }
            }
        }

        [TestMethod()]
        public void DeleteArticle_NonExisting_Id_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-deletion of article  
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                var result = (HttpNotFoundResult)articleController.Delete(455);

                articleRepository = new ArticlesRepository();
                existArticles = articleRepository.GetAllArticles().ToList();

                Assert.AreEqual(404, result.StatusCode); //404 - Page Not Found
                Assert.AreEqual(count, existArticles.Count()); //Record count should remain the same after editting
            }
        }

        [TestMethod()]
        public void DeleteArticle_OutOfBounds_Id_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-deletion of article  
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                var result = (HttpNotFoundResult)articleController.Delete(0);

                articleRepository = new ArticlesRepository();
                existArticles = articleRepository.GetAllArticles().ToList();

                Assert.AreEqual(404, result.StatusCode); //404 - Page Not Found
                Assert.AreEqual(count, existArticles.Count()); //Record count should remain the same after editting
            }
        }

        [TestMethod()]
        public void DeleteArticle_Negative_Id_Invalid()
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                articleRepository = new ArticlesRepository();
                int count = articleRepository.GetAllArticles().Count(); //Count articles pre-deletion of article  
                List<SMCC.Models.Article> existArticles = new List<SMCC.Models.Article>();

                var result = (HttpNotFoundResult)articleController.Delete(-1);

                articleRepository = new ArticlesRepository();
                existArticles = articleRepository.GetAllArticles().ToList();

                Assert.AreEqual(404, result.StatusCode); //404 - Page Not Found
                Assert.AreEqual(count, existArticles.Count()); //Record count should remain the same after editting
            }
        }
    }
}
