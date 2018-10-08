using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArticleWorkflow
{
    public class PublishedArticleState : ArticleState
    {
        public PublishedArticleState(Article article) : base(article)
        {
            // Update database to reflect article is published.
            // ################## TO BE DONE ####################
        }

        internal override void Submit()
        {
            throw new System.NotImplementedException();
        }

        internal override void Delete()
        {
            throw new System.NotImplementedException();
        }

        internal override void Update(string textToUpdate)
        {
            throw new System.NotImplementedException();
        }

        internal override void Publish()
        {
            throw new System.NotImplementedException();
        }

        internal override void Review()
        {
            throw new System.NotImplementedException();
        }
    }
}
