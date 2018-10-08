using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArticleWorkflow
{
    public class PublishAllArticleFactory : ArticleFactory
    {
        public List<ArticleComponent> articlesToPublish
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public bool PublishToAll()
        {
            throw new System.NotImplementedException();
        }
    }
}
