using Microsoft.Extensions.Caching.Memory;
using NLayer.Data;
using NLayer.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.CacheService {
  public class InMemoryCacheService {
    private readonly IMemoryCache _cache;
    private readonly string key = "mydata";
    public InMemoryCacheService(IMemoryCache memoryCache) {
      _cache = memoryCache;
    }
    public InMemoryCacheData Add(CacheItemPriority priority, DateTimeOffset dateTimeOffset) {
      InMemoryCacheData value;
      if (!_cache.TryGetValue(key, out value)) {
        //Burada cache için belirli ayarlamaları yapıyoruz.Cache süresi,önem derecesi gibi
        var cacheExpOptions = new MemoryCacheEntryOptions {
          //AbsoluteExpiration = DateTime.Now.AddMinutes(30),
          //Priority = CacheItemPriority.Normal
          AbsoluteExpiration = dateTimeOffset,
          Priority = priority
        };
        value = new InMemoryCacheData { FirstTime = DateTime.Now };
        //Bu satırda belirlediğimiz key'e göre ve ayarladığımız cache özelliklerine göre kategorilerimizi in-memory olarak cache'liyoruz.
        _cache.Set(key, value, cacheExpOptions);
        InMemoryCacheData retValue = Get();
        return retValue;
      }
      return value;
    }
    public InMemoryCacheData Get() {
      InMemoryCacheData value;
      if (!_cache.TryGetValue(key, out value)) {
        return null;
      }
      return value;
    }
    public void DeleteCache() {
      //Remove ile verilen key'e göre bulunan veriyi siliyoruz
      _cache.Remove(key);
    }
  }
}
