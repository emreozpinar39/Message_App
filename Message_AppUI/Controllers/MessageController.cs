using Message_AppUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Net.Http;

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

        /*SignalR */
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
    }
}
