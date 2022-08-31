using Message_App.Core.IConfiguration;
using Message_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Message_App.Controllers
{
    [ApiController]
    [Route("Message")]
    public class MessageController : ControllerBase
    {
        private readonly ILogger<MessageController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public MessageController(ILogger<MessageController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _unitOfWork.Messages.All();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessage(int id)
        {
            var message = await _unitOfWork.Messages.GetByMessageId(id);

            if (message == null)
            {
                return NotFound(); 
            }
            return Ok(message);

        }

        [HttpPost]
        public async Task<IActionResult> AddMessage(Message message)
        {
            if (ModelState.IsValid)
            {

                await _unitOfWork.Messages.Add(message);
                await _unitOfWork.CompleteAsync();

                return Ok(message);
            }
            return new JsonResult("Something went rong") { StatusCode = 500 };
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, Message message)
        {
            if (id != message.Id)
            {
                return BadRequest();
            }
            await _unitOfWork.Messages.Update(message);
            await _unitOfWork.CompleteAsync();

            return Content("Message updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _unitOfWork.Messages.GetByMessageId(id);

            if (message == null)
            {
                return BadRequest();
            }

            await _unitOfWork.Messages.DeleteMessage(id);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

    }
}
