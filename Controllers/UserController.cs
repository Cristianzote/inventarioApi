using inventarioApi.Data.Services;
using inventarioApi.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inventarioApi.Data.Responses;



namespace inventarioApi.Controllers
{
    [ApiController]
    [Route("/api/v1/user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        //GET
        [HttpGet("ok")]
        [AllowAnonymous]
        public ActionResult Get()
        {
            return Ok("Funciona");
        }

        [HttpGet("{ID_USER}")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> GetUser(int ID_USER)
        {
            var response = await _userService.GetUsersByIdAsync(ID_USER);

            if (response == null)
            {
                return NotFound("No users found");
            }
            return Ok(response);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllUsers()
        {
            var response = await _userService.GetUsersAsync();

            return Ok(response);
        }

        //POST
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateUser([FromBody] User USER)
        {
            var response = await _userService.CreateUser(USER);

            return CreatedAtAction(nameof(Get), "Ejemplo", response);
        }
    }
}
