using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArticleWorkflow
{
    public class NewArticleState : ArticleState
    {
        public NewArticleState(Article article) : base(article)
        {

        }

        internal override void Submit()
        {
            // Submit for Review

            // Article needs to be udpated in Database
            //Article.ArticleState = GetType().ToString();
            // We update article and set it as Saved to reflect new change from Saved state.

            // Change state to Saved
            this.Article.articleState = new SavedArticleState(this.Article);
        }

        internal override void Delete()
        {
            // Article must be removed from Database.
            // Object destroyed.
        }

        internal override void Update(string textToUpdate)
        {
            // Information should be submitted to database
            
        }

        internal override void Publish()
        {
            // NO ACTION - FUNCTION NOT ALLOWED IN THIS STATE
        }

        internal override void Review()
        {
            // NO ACTION - FUNCTION NOT ALLOWED IN THIS STATE
        }
    }
}
