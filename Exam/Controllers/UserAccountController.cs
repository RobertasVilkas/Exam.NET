using Exam.BLL;
using Exam.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountsController : ControllerBase
    {
        private readonly IUserAccountService _accountsService;
        private readonly IJwtService _jwtService;
        public UserAccountsController(IUserAccountService accountsService, IJwtService jwtService)
        {
            _accountsService = accountsService;
            _jwtService = jwtService;
        }

        [HttpPost("SignUp")]
        public ActionResult SignUp([FromBody] AuthorizationDto request)
        {
            try
            {
                _accountsService.SignupNewAccount(request.UserName, request.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Username is already taken");
            }
        }
        [HttpPost("Login")]
        public ActionResult Login([FromBody] AuthorizationDto request)
        {
            var (loginSuccess, account) = _accountsService.Login(request.UserName, request.Password);
            if (!loginSuccess)
            {
                return BadRequest("Wrong username or password");
            }
            else
            {
                var jwt = _jwtService.GetJwtToken(account.Username, account.Id, account.Role);
                return Ok(jwt);
            }
        }
    }
}