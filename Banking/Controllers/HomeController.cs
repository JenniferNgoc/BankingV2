using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Banking.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Authentication.SignOutAsync("Cookies");
            return View();
        }
        
        public IActionResult Error()
        {
            return View();
        }
    }
}
