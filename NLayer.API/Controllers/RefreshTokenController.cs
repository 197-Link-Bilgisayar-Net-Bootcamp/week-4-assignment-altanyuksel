using Microsoft.AspNetCore.Mvc;
using NLayer.Data.Models.JWTModels;
using NLayer.Service.TokenServices;

namespace NLayer.API.Controllers {
  [Route("api")]
  [ApiController]
  public class RefreshTokenController : ControllerBase {
		private readonly RefreshTokenService _refTokenService;
		public RefreshTokenController(RefreshTokenService tokenService) {
			this._refTokenService = tokenService;
		}
		[HttpPut("refreshToken")]
		public async Task< IActionResult> RefreshTokenAsync(Tokens tok) {
			var token = await _refTokenService.GetRefreshTokenAsync(tok);
			if (token == null) {
				return Unauthorized();
			}
			return Ok(token);
		}
	}
}