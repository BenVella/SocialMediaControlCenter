using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SMCC.Models
{
    public class ArticleReview
    {
        public Article articleModel { get; set; }
        public Review reviewModel { get; set; }
    }
}