using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArticleWorkflow
{
    public abstract class ArticleComponent
    {
        public abstract void Operation();

        protected ArticleComponent component
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        protected string author
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        protected DateTime dateOfCreation
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void GetAuthor()
        {
            throw new System.NotImplementedException();
        }

        
    }
}
