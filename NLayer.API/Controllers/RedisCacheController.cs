using Microsoft.AspNetCore.Mvc;
using NLayer.Service;
using NLayer.Service.Models;

namespace NLayer.API.Controllers {
  [ApiController]
  [Route("api")]
  public class RedisCacheController : ControllerBase {
    private readonly IRedisCacheService _cacheService;

    public RedisCacheController(IRedisCacheService cacheService) {
      _cacheService = cacheService;
    }
    [HttpPost("redisCache/{key}")]
    public async Task<IActionResult> Get(string key) {
      return Ok(await _cacheService.GetValueAsync(key));
    }

    [HttpPost("redisCache")]
    public async Task<IActionResult> Post([FromBody] RedisCacheData model) {
      await _cacheService.SetValueAsync(model.Key, model.Value);
      return Ok();
    }
    [HttpDelete("redisCache/{key}")]
    public async Task<IActionResult> Delete(string key) {
      await _cacheService.Clear(key);
      return Ok();
    }
  }
}
