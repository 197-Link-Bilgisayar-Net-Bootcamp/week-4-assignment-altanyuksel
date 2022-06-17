using NLayer.Data.Models.JWTModels;
using NLayer.Data.Repositories;
using NLayer.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.TokenServices {
  public class TokenService {
    private readonly IJWTManagerRepository _jWTManager;
    private readonly GenericRepository<User> _userRepository;
    public TokenService(IJWTManagerRepository jWTManager, GenericRepository<User> userRepository) {
      _jWTManager = jWTManager;
      _userRepository = userRepository;
    }
    public Task<List<User>> Get() {
      var userList = _userRepository.GetAllAsync();
      return userList;
    }
    public async Task<Tokens> AuthenticateAsyn(User usersdata) {
      var token = _jWTManager.AuthenticateAsync(usersdata);

      if (token == null) {
        return null;
      }
      return await token;
    }
  }
}