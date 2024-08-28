using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingAPI.Models;
using ParkingAPI.Services;
using System.Data;

namespace ParkingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration, UserService userService)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("GetUser")]
        public JsonResult GetUser()
        {
            DataTable table = _userService.GetAllUsers();
            return new JsonResult(table);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            DataTable table = _userService.GetUserById(id);

            if (table.Rows.Count == 0)
            {
                return NotFound(new { message = "User not found" });
            }

            return new JsonResult(table);
        }

        [Authorize]
        [HttpPost("AddUser")]
        public JsonResult AddUser(User user)
        {
            _userService.AddUser(user);
            return new JsonResult("User Added Successfully");
        }

        [Authorize]
        [HttpPut("UpdateUser")]
        public JsonResult UpdateUser(User user)
        {
            _userService.UpdateUser(user);
            return new JsonResult("User Updated Successfully");
        }

        [Authorize]
        [HttpDelete("DeleteUser/{id}")]
        public JsonResult DeleteUser(int id)
        {
            _userService.DeleteUser(id);
            return new JsonResult("User Deleted Successfully");
        }
    }
}
