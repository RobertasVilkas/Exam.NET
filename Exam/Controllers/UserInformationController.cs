using Exam.DAL;
using Exam.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Numerics;
using System.Security.Claims;

namespace Exam.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserInformationController : ControllerBase
    {
        private readonly IUserInformationDbRepository _userInformation;

        public UserInformationController(IUserInformationDbRepository userInformation)
        {
            _userInformation = userInformation;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User, Admin")]
        [HttpPost]
        [Route("add-user-information")]
        public IActionResult PostItem(UserInformationDto infoToAdd)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var useridInt = int.Parse(userIdStr);

            _userInformation.AddNewUserToList(useridInt, infoToAdd);
            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User, Admin")]
        [HttpPut]
        [Route("update-user-information")]
        public IActionResult UpdateItem([FromBody] PersonalInformationDto human)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var useridInt = int.Parse(userIdStr);
            _userInformation.UpdateUserById(useridInt, human.Name, human.Surname, human.PersonalCode,
                human.TelephoneNumber, human.Email, human.Address.City, human.Address.Street,
                human.Address.HouseNumber, human.Address.FlatNumber);

            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _userInformation.DeleteUserById(id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("get-all-users-information")]
        public IActionResult GetAllUserInformation()
        {
            var allUserInfo = _userInformation.GetAllUserInfo();
            return Ok(allUserInfo);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User, Admin")]
        [HttpGet("user-information")]
        public IActionResult GetUserInformation()
        {
            var userIdStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr))
            {
                return BadRequest("User ID not found in authentication token.");
            }

            var useridInt = int.Parse(userIdStr);
            var userInfo = _userInformation.GetUserInformationById(useridInt);
            return Ok(userInfo);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User, Admin")]
        [HttpGet("get-all-accounts")]
        public List<UserInformationDto> GetAllAccounts()
        {
            return _userInformation.GetAllAccounts();
        }
    }
}