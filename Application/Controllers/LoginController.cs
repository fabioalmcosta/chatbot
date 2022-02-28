using Domain.Models.User;
using Infrastructure.Exception;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Service.Services;
using Service.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private IUserService _userService;
        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index([FromBody] PostLoginModel model)
        {
            var result = await _userService.Login(model);
            return Ok(result);
        }
    }
}
