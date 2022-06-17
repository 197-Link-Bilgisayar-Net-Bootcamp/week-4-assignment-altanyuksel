using NLayer.Data.Models.JWTModels;
using NLayer.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.TokenServices {
  public class RefreshTokenService {
    private readonly IJWTManagerRepository _jWTManager;
    public RefreshTokenService(IJWTManagerRepository jWTManager) {
      _jWTManager = jWTManager;
    }
    public async ValueTask<Tokens> GetRefreshTokenAsync(Tokens tok) {
      var token = _jWTManager.GetRefreshTokenAsync(tok);

      if (token == null) {
        return null;
      }
      return await token;
    }
  }
}
