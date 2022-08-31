using Microsoft.AspNetCore.Mvc;

namespace Message_AppUI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
