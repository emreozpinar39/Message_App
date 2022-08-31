using Message_AppUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Message_AppUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Users user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9325/");
                var postTask = client.GetAsync($"UserLogin/{user.FirstName}/{user.Password}");
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readJsonTask = result.Content.ReadAsAsync<Users>();
                    readJsonTask.Wait();
                    Users usr = readJsonTask.Result;

                    var claims = new List<Claim>()
                    {
                        new Claim("ID",usr.Id.ToString()),
                        new Claim(ClaimTypes.Name,usr.FirstName),
                        new Claim(ClaimTypes.Surname,usr.LastName),
                        new Claim(ClaimTypes.Email,usr.Email)
                    };

                    var identityUser = new ClaimsIdentity(claims, "Login");
                    ClaimsPrincipal principal = new ClaimsPrincipal(identityUser);
                    await HttpContext.SignInAsync(principal);

                    if (usr.FirstName == "admin" && user.Password ==usr.Password)
                    {
                        return RedirectToAction("Index", "User");
                    }

                    return RedirectToAction("Index", "Message");
                }
                else
                {
                    TempData["mesajHata"] = "Username or password is wrong!";
                    return RedirectToAction("Login", "Home");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Message");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
