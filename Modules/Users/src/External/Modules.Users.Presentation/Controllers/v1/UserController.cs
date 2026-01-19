using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Common.Presentation;

namespace Modules.Users.Presentation.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost("forget-password/email")]
        public Task<ActionResult<Guid>> ForgetPasswordByEmail([FromBody] ForgetPasswordRequestEmailDto requestDto)
        {
            throw new NotImplementedException();
        }
        [HttpPost("forget-password/phone")]
        public Task<ActionResult<Guid>> ForgetPasswordByPhone([FromBody] ForgetPasswordRequestPhoneDto requestDto)
        {
            throw new NotImplementedException();
        }
        [HttpPost("register")]
        public async Task<ActionResult<Guid>> CreateUser([FromBody] CreateUserRequestDto requestDto)
        {
            var userid = await userService.CreateUserAsync(requestDto);
            return Ok(userid);
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginUserResponse>> LoginUser([FromBody] LoginUserRequestDto requestDto)
        {
            var loginUserResponse = await userService.LoginUserAsync(requestDto);
            return Ok(loginUserResponse);
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginUserResponse>> RefreshToken([FromBody] RefreshTokenRequestDto requestDto)
        {
            var loginUserResponse = await userService.RefreshUserAsnc(requestDto);
            return Ok(loginUserResponse);
        }
    }
}