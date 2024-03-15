using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using Microsoft.Identity.Web.Resource;
using System.Diagnostics;
using WebApp_OpenIDConnect_DotNet.Models;

namespace WebApp_OpenIDConnect_DotNet.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Login(string returnUrl = "/signin-oidc")
        {
            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl });
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Claims()
        {
            return View();
        }

        [Authorize]
        public IActionResult FindAJob()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string message)
        {

            var errorMessage = HttpContext.Items["ErrorMessage"] as string;
            return View(new ErrorViewModel {RequestId = errorMessage ?? HttpContext.TraceIdentifier});
           // return View(new ErrorViewModel { userMessage = "Something went wrong please try again later" });
        }
    }
}