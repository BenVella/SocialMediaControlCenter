using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMCC.Models;

namespace SMCC.DataAccess
{
    public class ArticlesRepository : ConnectionClass
    {
        public ArticlesRepository()
            : base()
        { }

        public IEnumerable<Article> GetAllArticles()
        {
            return Entity.Articles.ToList();
        }

        public Article GetArticle(int id)
        {
            return Entity.Articles.Find(id);
        }
    }
}