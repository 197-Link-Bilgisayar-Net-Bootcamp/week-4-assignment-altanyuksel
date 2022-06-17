using NLayer.Data.Models.JWTModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Data.Repositories {
  public interface IJWTManagerRepository {
    ValueTask<Tokens> AuthenticateAsync(User users);
    ValueTask<Tokens> GetRefreshTokenAsync(Tokens tok);
  }
}