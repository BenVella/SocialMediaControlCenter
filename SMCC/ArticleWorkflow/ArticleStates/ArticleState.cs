using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArticleWorkflow
{
    public abstract class ArticleState
    {
        protected Article Article;

        internal ArticleState(Article article)
        {
            this.Article = article;
        }

        internal abstract void Submit();

        internal abstract void Update(string textToUpdate); 

        internal abstract void Delete();

        internal abstract void Review();

        internal abstract void Publish();
    }
}
