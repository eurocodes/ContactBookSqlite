using ContactBook.DataAccess.Interfaces;
using ContactBook.ModelDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;

        public AuthController(IUserRepository userRepository, IAuthRepository authRepository, IConfiguration config)
        {
            _userRepository = userRepository ??
               throw new ArgumentNullException(nameof(userRepository));
            _authRepository = authRepository ??
                throw new ArgumentNullException(nameof(authRepository));
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto userDto)
        {
            var result = await _userRepository.RegisterUser(userDto);
            if (result == null)
                return BadRequest("Invalid request");

            return Created("", result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _authRepository.Login(model.Email, model.Password, model.RememberMe);
            if (user == null)
                return Unauthorized(model);

            ICollection<string> roles = await _authRepository.GetUserRoles(user);

            var token = UtilityClass.GenerateToken(user.Id, user.UserName, user.FirstName, user.LastName, user.Email, _config, roles);

            return Ok(token);
        }

        [HttpGet("{userId}")]
        public IActionResult GetUser(string userId)
        {
            var user = _userRepository.GetUser(userId);
            if (user == null)
                return BadRequest("Invalid request");

            return Ok(user);
        }
    }
}
