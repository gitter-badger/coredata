using Microsoft.AspNetCore.Http;
using CoreData.Models;
using CoreData.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CoreData.Controllers
{

    public class DiagnosticsController : Controller
    {
        public IActionResult Index()
        {
            var model = new Diagnostics();

            // What is the browser?
            model.BrowserName = HttpContext.Request.UserAgent();

            // Is it mobile browser?
            if (HttpContext.Request.IsMobileBrowser())
            {
                model.IsMobileBrowser = "Yes";
            }
            else
            {
                model.IsMobileBrowser = "No";
            }

            // What is the IP address?
            model.IpAddress = HttpContext.Connection.RemoteIpAddress.ToString();

            return View(model);
        }
    }


}
