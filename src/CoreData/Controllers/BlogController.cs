using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CoreData.Models;
using CoreData.ViewModels;
using cloudscribe.Web.Pagination;

namespace CoreData.Controllers
{
    public class BlogController : Controller
    {
        private const int DefaultPageSize = 10;
        private List<Article> allArticles = new List<Article>();
        private readonly string[] allCategories = new string[3] { "DIY", "Hacks", "L33t" };

        public BlogController()
        {
            InitializeBlog();
        }

        private void InitializeBlog()
        {
            // Create a random list of articles
            // 50% of them are in the DIY category, 25% in the Hacks 
            // category and 25% in the L33t category.
            for (var i = 0; i < 527; i++)
            {
                var article = new Article();
                article.Name = "News " + (i + 1);
                var categoryIndex = i % 4;
                if (categoryIndex > 2)
                {
                    categoryIndex = categoryIndex - 3;
                }
                article.Category = allCategories[categoryIndex];
                allArticles.Add(article);
            }
        }


        // [Route("blog/{page?}")]
        public IActionResult Index(int? page)
        {
            var currentPageNum = page.HasValue ? page.Value : 1;
            var offset = (DefaultPageSize * currentPageNum) - DefaultPageSize;
            var model = new ArticleListViewModel();

            model.Articles = this.allArticles
                .Skip(offset)
                .Take(DefaultPageSize)
                .ToList();

            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = DefaultPageSize;        
            model.Paging.TotalItems = allArticles.Count;

            return View(model);
        }


        public IActionResult ArticleList(int? pageNumber, int? pageSize, string query = "")
        {

            var itemsPerPage = pageSize.HasValue ? pageSize.Value : DefaultPageSize;
            var currentPageNum = pageNumber.HasValue ? pageNumber.Value : 1;
            var offset = (itemsPerPage * currentPageNum) - itemsPerPage;

            var model = new ArticleListViewModel();
            var filtered = this.allArticles.Where(p =>
                p.Category.StartsWith(query));

            model.Articles = filtered
                .Skip(offset)
                .Take(itemsPerPage)
                .ToList();

            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = filtered.ToList().Count;

            model.Query = query; //TODO: sanitize
 
            return View(model);
        }



        public IActionResult BlogPagingDemo(int? pageNumber,string query = "")
        {        
            int itemsPerPage = 1;
            var currentPageNum = pageNumber.HasValue ? pageNumber.Value : 1;
            var offset = (itemsPerPage * currentPageNum) - itemsPerPage;

            var filtered = allArticles.Where(p =>
                p.Category.StartsWith(query)
            )

            .OrderByDescending(p => p.CreatedUtc)    
            .ToList();  

            var model = new ArticleListViewModel();
            model.Articles = filtered
            .Skip(offset)        
            .Take(itemsPerPage)
            .ToList();

            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = itemsPerPage;
            model.Paging.TotalItems = filtered.Count;
            //model.Paging.UseReverseIncrement = true;

            model.Query = query; //TODO: sanitize

            return View(model);
        }



        public IActionResult ViewByCategory(string categoryName, int? page)
        {
            categoryName = categoryName ?? this.allCategories[0];
            var currentPageNum = page.HasValue ? page.Value : 1;
            var offset = (DefaultPageSize * currentPageNum) - DefaultPageSize;

            var filtered = this.allArticles.Where(p =>
                p.Category.Equals(categoryName)
                ).ToList();

            var model = new ArticleListViewModel();

            model.Articles = filtered
                .Skip(offset)
                .Take(DefaultPageSize)
                .ToList();

            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.TotalItems = filtered.Count;

      
            ViewBag.CategoryName = new SelectList(this.allCategories, categoryName);
            ViewBag.CategoryDisplayName = categoryName;

            return View("ArticleByCategory", model);

        }


        public IActionResult ViewByCategories(string[] categories, int? page)
        {
            // I have not figured out how to convert a string array to routeparam
            // in mvc6 yet so the categories may be passed as a single csv string

            if (categories != null)
            {
                if ((categories.Length == 1) && (categories[0].Contains(",")))
                {
                    categories = categories[0].Split(',');
                }
            }

            var currentPageNum = page.HasValue ? page.Value : 1;
            var offset = (DefaultPageSize * currentPageNum) - DefaultPageSize;

            var model = new ViewByCategoriesViewModel();
            model.Categories = categories ?? new string[0];
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            var filtered = this.allArticles.Where(p =>
                model.Categories.Contains(p.Category)
                ).ToList();

            model.Articles = filtered
                .Skip(offset)
                .Take(DefaultPageSize)
                .ToList(); 

            model.AvailableCategories = this.allCategories; 
            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.TotalItems = filtered.Count;
            model.Paging.ShowFirstLast = true;

            return View("ArticlesByCategories", model);
        }

        public IActionResult IndexAjax()
        {
            var model = new ArticleListViewModel();

            model.Articles = this.allArticles
                .Take(DefaultPageSize)
                .ToList();

            model.Paging.CurrentPage = 1;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.TotalItems = allArticles.Count;

            if (HttpContext.Request.IsAjaxRequest())
            {
                return PartialView("_PagingModal", model);
            }

            return View(model);
        }



        //[Route("paging/ajaxpage/{page?}")]
        public IActionResult AjaxPage(int? page)
        {
            ViewBag.Title = "Browse all articles";
        
            var currentPageNum = page.HasValue ? page.Value : 1;
            var offset = (DefaultPageSize * currentPageNum) - DefaultPageSize;

            var model = new ArticleListViewModel();
            model.Articles = this.allArticles  
                .Skip(offset)
                .Take(DefaultPageSize)
                .ToList();

            model.Paging.CurrentPage = currentPageNum;
            model.Paging.ItemsPerPage = DefaultPageSize;
            model.Paging.TotalItems = allArticles.Count;

            return PartialView("_ArticleGrid", model);
        }
        
    }
}