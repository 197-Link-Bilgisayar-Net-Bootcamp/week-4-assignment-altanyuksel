using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NLayer.Data.Models.JWTModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Data.Repositories {
  public class JWTManagerRepository : IJWTManagerRepository {
    private readonly IConfiguration _iconfiguration;
		private readonly GenericRepository<User> _userRepository;
		private readonly UnitOfWork _unitOfWork;
		private readonly int _tokenExpMinutes = 10;
		private readonly int _refreshTokenExpMinutes = 30;
		private List<User> _listUserAll = new();

		public JWTManagerRepository(IConfiguration iconfiguration, GenericRepository<User> userRepository, UnitOfWork unitOfWork) {
      this._iconfiguration = iconfiguration;
      this._userRepository = userRepository;
      _unitOfWork = unitOfWork;
    }

    public async ValueTask<Tokens> AuthenticateAsync(User users) {
			SetUserList();
			var tempU = _listUserAll.Where(x => x.Name == users.Name & x.Password == users.Password);
			if (tempU is null) {
				return null;
			}
			users = tempU.First();
      // Else we generate JSON Web Token
      var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Secret"]);
			var tokenDescriptor = new SecurityTokenDescriptor {
				Subject = new ClaimsIdentity(new Claim[]
				{
			 new Claim(ClaimTypes.Name, users.Name)
				}),
				Expires = DateTime.Now.AddMinutes(_tokenExpMinutes),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);

			var refreshToken = GenerateRefreshToken();
			users.RefreshToken = refreshToken;
			users.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_refreshTokenExpMinutes);
			await _userRepository.UpdateAysnc(users);
      await _unitOfWork.CommitAsyn();
			return new Tokens { Token = tokenHandler.WriteToken(token), RefreshToken = refreshToken };
		}
		public void SetUserList() {
			_listUserAll = _userRepository.GetAll();
		}
		public async ValueTask<Tokens> GetRefreshTokenAsync(Tokens tok) {
      if (tok is null) {
				return null;
      }
			string accessToken = tok.Token;
			string refreshToken = tok.RefreshToken;

			var principal = GetPrincipalFromExpiredToken(accessToken);
			if (principal == null) {
				return null;
			}
			string username = principal.Identity.Name;
      var listuser = await _userRepository.GetItemsAsync(e => e.Name == username);
      if (listuser == null || listuser.Count == 0) {
        return null;
      }
      var user = listuser[0];

      //var user = new User {	RefreshToken = refreshToken, Password = accessToken };

      if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now) {
				return null;
			}
			var newAccessToken = CreateToken(principal.Claims.ToList());
			var newRefreshToken = GenerateRefreshToken();

			user.RefreshToken = newRefreshToken;
			user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(_refreshTokenExpMinutes); 
			await _userRepository.UpdateAysnc(user);
			await _unitOfWork.CommitAsyn();
			tok = new Tokens();
			tok.Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
			tok.RefreshToken = newRefreshToken;
			return tok;
		}
		private JwtSecurityToken CreateToken(List<Claim> authClaims) {
			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iconfiguration["JWT:Secret"]));
			_ = int.TryParse(_iconfiguration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

			var token = new JwtSecurityToken(
					issuer: _iconfiguration["JWT:ValidIssuer"],
					audience: _iconfiguration["JWT:ValidAudience"],
					expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
					claims: authClaims,
					signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
					);
			return token;
		}
		private string GenerateRefreshToken() {
			var randomNumber = new byte[32];
			using (var rng = RandomNumberGenerator.Create()) {
				rng.GetBytes(randomNumber);
				return Convert.ToBase64String(randomNumber);
			}
		}
		private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token) {
			var tokenValidationParameters = new TokenValidationParameters {
				ValidateIssuer = false,
				ValidateAudience = false,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ValidIssuer = _iconfiguration["JWT:ValidIssuer"],
				ValidAudience = _iconfiguration["JWT:ValidAudience"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iconfiguration["JWT:Secret"]))
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
			if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				throw new SecurityTokenException("Invalid token");

			return principal;
		}
  }
}