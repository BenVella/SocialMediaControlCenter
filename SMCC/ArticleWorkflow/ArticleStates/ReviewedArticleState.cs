using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArticleWorkflow
{
    public class ReviewedArticleState : ArticleState
    {
        public ReviewedArticleState(Article article) : base(article)
        {
            // Update database to reflect article is reviewed.
            // ################## TO BE DONE ####################
        }

        internal override void Submit()
        {
            // NO ACTION - FUNCTION NOT ALLOWED IN THIS STATE
        }

        internal override void Delete()
        {
            // NO ACTION - FUNCTION NOT ALLOWED IN THIS STATE
        }

        internal override void Update(string textToUpdate)
        {
            // NO ACTION - FUNCTION NOT ALLOWED IN THIS STATE
        }

        internal override void Publish()
        {
            throw new System.NotImplementedException();
        }

        internal override void Review()
        {
            // NO ACTION - FUNCTION NOT ALLOWED IN THIS STATE
        }
    }
}
