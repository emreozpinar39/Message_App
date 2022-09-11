using Message_AppUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Message_AppUI.Controllers
{
    public class MessageController : Controller
    {
        private readonly IHubContext<SignalServer> _signalRHub;
        public MessageController(IHubContext<SignalServer> signalRHub)
        {
            _signalRHub = signalRHub;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// SignalR
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMessages()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:9325/");
            var getTask = client.GetAsync("Message");
            getTask.Wait();
            var result = getTask.Result;
            var readJsonTask = result.Content.ReadAsAsync<IEnumerable<Message>>();
            readJsonTask.Wait();
            var messages = readJsonTask.Result;


            var getUsers = client.GetAsync("Users");
            getUsers.Wait();
            var usersResult = getUsers.Result;
            var readJsonUsers = usersResult.Content.ReadAsAsync<IEnumerable<Users>>();
            readJsonUsers.Wait();
            var users = readJsonUsers.Result;

            foreach (var message in messages)
            {
                foreach (var user in users)
                {
                    if (message.UserId == user.Id)
                    {
                        message.User = user;
                        break;
                    }
                }
            }

            return Ok(messages);

        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:9325/");
                var getTask = client.GetAsync($"Message/{id}");
                getTask.Wait();

                var result = getTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readJsonTask = result.Content.ReadAsAsync<Message>();
                    readJsonTask.Wait();
                    var message = readJsonTask.Result;

                    var getUser = client.GetAsync($"Users/{message.UserId}");
                    getUser.Wait();
                    var userResult = getUser.Result;
                    var readJsonUser = userResult.Content.ReadAsAsync<Users>();
                    readJsonUser.Wait();
                    var user = readJsonUser.Result;

                    message.User = user;

                    return View(message);
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
            Guid _userId = Guid.Parse(User.FindFirstValue("ID"));
            Message message = new Message { UserId = _userId };
            return View(message);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Message message)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9325/");
                var postTask = client.PostAsJsonAsync($"Message", message);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    await _signalRHub.Clients.All.SendAsync("LoadMessages");
                    return RedirectToAction(nameof(Index));
                }

                TempData["messageFail"] = "Message Adding Error!";
                return View(message);

            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("http://localhost:9325/");
                var getTask = client.GetAsync($"Message/{id}");
                getTask.Wait();

                var result = getTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readJsonTask = result.Content.ReadAsAsync<Message>();
                    readJsonTask.Wait();

                    if (readJsonTask.Result.UserId != Guid.Parse(User.FindFirstValue("ID")))
                    {
                        TempData["messageFail"] = "You can only edit your own message!";

                        return View("Index");
                    }

                    var message = readJsonTask.Result;

                    var getUser = client.GetAsync($"Users/{message.UserId}");
                    getUser.Wait();
                    var userResult = getUser.Result;
                    var readJsonUser = userResult.Content.ReadAsAsync<Users>();
                    readJsonUser.Wait();
                    var user = readJsonUser.Result;

                    message.User = user;
                    return View(message);
                }
                else
                {
                    return Content("No Content Found!");
                }
            }

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(Message message)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9325/");
                var postTask = client.PostAsJsonAsync($"Message/{message.Id}", message);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    await _signalRHub.Clients.All.SendAsync("LoadMessages");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["messageFail"] = "An error occurred while editing the message!";
                    return View(message);
                }

            }
        }


        [HttpGet]
        [Authorize]
        public IActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9325/");
                var getTask = client.GetAsync($"Message/{id}");
                getTask.Wait();

                var result = getTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readJsonTask = result.Content.ReadAsAsync<Message>();
                    readJsonTask.Wait();

                    if (readJsonTask.Result.UserId != Guid.Parse(User.FindFirstValue("ID")))
                    {
                        TempData["messageFail"] = "You can only delete your own message!";

                        return View("Index");
                    }
                    Message message = readJsonTask.Result;
                    Users user = new Users()
                    {
                        FirstName = User.Identity.Name,
                        LastName = User.FindFirst(ClaimTypes.Surname).Value,
                        Email = User.FindFirst(ClaimTypes.Email).Value
                    };
                    message.User = user;
                    return View(message);
                }
                else
                {
                    return Content("No Content Found!");
                }
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id, Guid userId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9325/");
                var delTask = client.DeleteAsync($"Message/{id}");
                delTask.Wait();

                var result = delTask.Result;
                if (!result.IsSuccessStatusCode)
                {
                    TempData["messageFail"] = "Message Deletion Failed!";
                    return View();
                }

                await _signalRHub.Clients.All.SendAsync("LoadMessages");
                return View("Index");
            }
        }

    }
}
