using UserManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Models;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@techhive.com", Department = "IT" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers() => Ok(_users);

        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return user == null ? NotFound() : Ok(user);
        }

        //[HttpPost]
        //public ActionResult<User> CreateUser(User user)
        //{
        //    user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
        //    user.CreatedAt = DateTime.Now;
        //    _users.Add(user);
        //    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        //}

        //[HttpPut("{id}")]
        //public IActionResult UpdateUser(int id, User updatedUser)
        //{
        //    var user = _users.FirstOrDefault(u => u.Id == id);
        //    if (user == null) return NotFound();

        //    user.FirstName = updatedUser.FirstName;
        //    user.LastName = updatedUser.LastName;
        //    user.Email = updatedUser.Email;
        //    user.Department = updatedUser.Department;

        //    return NoContent();
        //}

        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            try
            {
                if (user == null) return BadRequest("User data is null.");

                if (!ModelState.IsValid) return BadRequest(ModelState);

                // Bug Fix: Check for duplicate email
                if (_users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    return BadRequest("A user with this email already exists.");
                }

                user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
                user.CreatedAt = DateTime.Now;
                _users.Add(user);

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                // Bug Fix: Handle unexpected crashes
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            try
            {
                if (updatedUser == null) return BadRequest();

                var user = _users.FirstOrDefault(u => u.Id == id);
                if (user == null) return NotFound($"User with ID {id} not found.");

                // Bug Fix: Validation check
                if (!ModelState.IsValid) return BadRequest(ModelState);

                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Email = updatedUser.Email;
                user.Department = updatedUser.Department;

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            _users.Remove(user);
            return NoContent();
        }
    }
}