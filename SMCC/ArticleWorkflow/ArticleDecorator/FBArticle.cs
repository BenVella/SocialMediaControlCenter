using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArticleWorkflow
{
    public class FBArticleDecorator : ArticleDecorator
    {
        public int addedUrl;

        public FBArticleDecorator(ArticleComponent component)
        {
            this.ArticleComponent = component;
        }
    
        public override void Operation()
        {
            throw new System.NotImplementedException();
        }
    }
}
