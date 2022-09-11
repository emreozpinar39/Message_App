using Message_AppUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Message_AppUI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9325/");
                var getTask = client.GetAsync("Users");
                getTask.Wait();

                var result = getTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readJsonTask = result.Content.ReadAsAsync<IEnumerable<Users>>();
                    readJsonTask.Wait();
                    var users = readJsonTask.Result;



                    return View(users);
                }
                else
                {
                    return Content("No Content Found!");
                }
            }
        }


        [HttpGet]
        [Authorize]
        public IActionResult Details(Guid id)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:9325/");
                var getTask = client.GetAsync($"Users/{id}");
                getTask.Wait();

                var result = getTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readJsonTask = result.Content.ReadAsAsync<Users>();
                    readJsonTask.Wait();
                    var user = readJsonTask.Result;


                    return View(user);
                }
                else
                {
                    return Content("No Content Found!");
                }
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Users user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9325/");
                var postTask = client.PostAsJsonAsync($"Users", user);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                TempData["userFail"] = "User Adding Error!";
                return View(user);

            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(Guid id)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:9325/");
                var getTask = client.GetAsync($"Users/{id}");
                getTask.Wait();

                var result = getTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readJsonTask = result.Content.ReadAsAsync<Users>();
                    readJsonTask.Wait();
                    var user = readJsonTask.Result;


                    return View(user);
                }
                else
                {
                    return Content("No Content Found!");
                }
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(Users user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9325/");
                var postTask = client.PostAsJsonAsync($"Users/{user.Id}", user);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["userFail"] = "An error occurred while editing the user!";
                    return View(user);
                }
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Delete(Guid id)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:9325/");
                var getTask = client.GetAsync($"Users/{id}");
                getTask.Wait();

                var result = getTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readJsonTask = result.Content.ReadAsAsync<Users>();
                    readJsonTask.Wait();
                    var user = readJsonTask.Result;


                    return View(user);
                }
                else
                {
                    return Content("No Content Found!");
                }
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(Guid id, string name)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9325/");
                var delTask = client.DeleteAsync($"Users/{id}");
                delTask.Wait();

                var result = delTask.Result;
                if (!result.IsSuccessStatusCode)
                {
                    TempData["userFail"] = "User Deletion Failed!";
                    return View();
                }

                return RedirectToAction(nameof(Index));
            }
        }
    }
}
