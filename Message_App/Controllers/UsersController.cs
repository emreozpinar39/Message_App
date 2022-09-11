using Message_App.Core.IConfiguration;
using Message_App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Message_App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _unitOfWork.Users.All();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _unitOfWork.Users.GetById(id);

            if (user == null)
            {
                return NotFound(); 
            }
            return Ok(user);

        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(Users user)
        {
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid();

                await _unitOfWork.Users.Add(user);
                await _unitOfWork.CompleteAsync();

                return Ok(user);
            }
            return new JsonResult("Something Went Wrong!") { StatusCode = 500 };
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, Users user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            await _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();

            return Content("User Update Completed");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _unitOfWork.Users.GetById(id);

            if (user == null)
            {
                return BadRequest();
            }

            await _unitOfWork.Users.Delete(id);
            await _unitOfWork.Messages.DeleteByUserId(id);
            await _unitOfWork.CompleteAsync();

            return Content("User Delete Completed");
        }

    }
}
