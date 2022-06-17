using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Data.Models;
using NLayer.Service.CacheService;

namespace NLayer.API.Controllers {
  [Route("api")]
  [ApiController]
  public class InMemoryCacheController : ControllerBase {
    private readonly IMemoryCache _cache;
    private readonly InMemoryCacheService _cacheService;
    public InMemoryCacheController(IMemoryCache memoryCache, InMemoryCacheService cacheService) {
      _cache = memoryCache;
      _cacheService = cacheService;
    }
    [HttpPost("inMemoryCache")]
    public IActionResult Add() {
      InMemoryCacheData retValue = _cacheService.Add(CacheItemPriority.Normal, DateTime.Now.AddMinutes(30));
      return Ok(retValue);
    }
    [HttpGet("inMemoryCache")]
    public IActionResult Get() {
      InMemoryCacheData data = _cacheService.Get();
      return Ok(data);
    }
    [HttpDelete("inMemory/{key}")]
    public IActionResult Delete() {
      _cacheService.DeleteCache();
      return Ok();
    }
  }
}
