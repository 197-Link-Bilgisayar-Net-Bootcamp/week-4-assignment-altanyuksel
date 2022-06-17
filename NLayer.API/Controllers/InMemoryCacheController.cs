using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Data.Models;
using NLayer.Service;

namespace NLayer.API.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class InMemoryCacheController : ControllerBase {
    private readonly IMemoryCache _cache;
    private readonly InMemoryCacheService _cacheService;
    public InMemoryCacheController(IMemoryCache memoryCache, InMemoryCacheService cacheService) {
      _cache = memoryCache;
      _cacheService = cacheService;
    }
    [HttpPost]
    public IActionResult Add() {
      InMemoryCacheData retValue = _cacheService.Add(CacheItemPriority.Normal, DateTime.Now.AddMinutes(30));
      return Ok(retValue);
    }
    [HttpGet]
    public IActionResult Get() {
      InMemoryCacheData data = _cacheService.Get();
      return Ok(data);
    }
    [HttpDelete]
    public IActionResult Delete() {
      _cacheService.DeleteCache();
      return Ok();
    }
  }
}
