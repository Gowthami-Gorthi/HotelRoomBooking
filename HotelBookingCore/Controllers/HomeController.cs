using HotelBookingCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HotelRoomBooking;

namespace HotelBookingCore.Controllers
{
    /*By installing System.Configuration.ConfigurationManager 
     * we can resolve the error .....??
     */
    public class HomeController : Controller
    {
        Class1 cls=new Class1();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(customer c, string cpwd)
        {
            if (ModelState.IsValid)
            {
                string s=cls.Register(c, cpwd); 
                try
                {
                    int x=int.Parse(s);
                    ViewData["Message"] = "Registraion successfully";
                }catch(FormatException e)
                {
                    ViewData["Message"] = s;
                }
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string emailId,string cpassword)
        {
            if(cls.Login(emailId, cpassword))
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
