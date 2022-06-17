using Microsoft.AspNetCore.Mvc;
using NLayer.Data.Models.JWTModels;
using NLayer.Service;
using NLayer.Service.Dtos;

namespace NLayer.API.Controllers {
  [Route("api")]
  [ApiController]
  public class TokenController : ControllerBase {
    private readonly TokenService _tokenService;
    public TokenController(TokenService tokenService) {
      this._tokenService = tokenService;
    }
		[HttpPost("token")]
		public IActionResult Authenticate(UserDto user) {
			User usersdata = new User() { Name = user.Name, Password = user.Password};
			//TokenService.Authenticate() metodunun tipi Task ise return değerini .Result diyerek alıyoruz.
			//TokenService.Authenticate() metodunun tipi ValueTask ise direkt return'u alabiliriz. .Result demeye gerek kalmaz.
			var token = _tokenService.AuthenticateAsyn(usersdata).Result;
			if (token == null) {
				return Unauthorized();
			}
			return Ok(token);
		}
	}
}
