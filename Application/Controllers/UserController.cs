using Domain.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Services;
using Service.Services.Interfaces;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromBody] PostUserModel model)
        {
            var result = await _userService.Post(model);
            return Ok(result);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var result = await _userService.Get(id);
            return Ok(result);
        }
    }
}

