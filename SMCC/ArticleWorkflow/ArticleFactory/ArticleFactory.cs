using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArticleWorkflow
{
    public abstract class ArticleFactory
    {
        public ArticleComponent ArticleComponent
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    
        public void GetArticle()
        {
            throw new System.NotImplementedException();
        }
    }
}
