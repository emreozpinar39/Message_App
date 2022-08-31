using Microsoft.AspNetCore.Mvc;

namespace Message_AppUI.Controllers
{
    public class MessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
