using Domain.Models.ChatMessage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ChatMessageController : ControllerBase
    {
        private IChatMessageService _chatService;
        public ChatMessageController(IChatMessageService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] ChatMessagePost model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _chatService.PostMessage(userId, model);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _chatService.GetMessages(userId, id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetConnectedUsers()
        {
            var result = await _chatService.GetConnectedUsers();
            return Ok(result);
        }

    }
}
