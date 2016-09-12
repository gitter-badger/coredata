using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreData.Models;
using cloudscribe.Web.Pagination;

namespace CoreData.ViewModels
{
    public class ViewByCategoriesViewModel
    {
        public ViewByCategoriesViewModel()
        {
            Paging = new PaginationSettings();
        }

        public List<Article> Articles { get; set; }
        public string[] AvailableCategories { get; set; }
        public string[] Categories { get; set; }
        public PaginationSettings Paging { get; set; }
    }
}
