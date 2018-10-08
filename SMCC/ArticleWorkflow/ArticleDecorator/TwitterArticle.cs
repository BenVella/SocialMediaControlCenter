using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArticleWorkflow
{
    public class TwitterArticleDecorator : ArticleDecorator
    {
        public string addedTags;

        public TwitterArticleDecorator(ArticleComponent component)
        {
            this.ArticleComponent = component;
        }

        public override void Operation()
        {
            throw new System.NotImplementedException();
        }
    }
}
