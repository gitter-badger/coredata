using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using cloudscribe.Web.Pagination;
using CoreData.Models;

namespace CoreData.ViewModels
{
    public class ArticleListViewModel
    {
        public ArticleListViewModel()
        {
            Paging = new PaginationSettings();
        }

        public string Query { get; set; } = string.Empty;

        public List<Article> Articles { get; set; } = null;

        public PaginationSettings Paging { get; set; }
    }
}
