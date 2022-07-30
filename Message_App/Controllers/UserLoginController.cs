using Message_App.Core.IConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Message_App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserLoginController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserLoginController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{Name}/{password}")]
        public async Task<IActionResult> CheckUserByNameAndPassword(string name, string password)
        {
            var user = await _unitOfWork.Users.GetByNameAndPassword(name, password);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
