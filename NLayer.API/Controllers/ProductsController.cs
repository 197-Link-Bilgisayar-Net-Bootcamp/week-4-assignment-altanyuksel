using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLayer.Service;

namespace NLayer.API.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase {
    private readonly ProductService _productService;
    public ProductsController(ProductService productService) {
      this._productService = productService;
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync() {
      var response = await _productService.GetAllAsync();

      return new ObjectResult(response) { StatusCode = response.Status };
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id) {
      var response = await _productService.GetAsync(id);

      return new ObjectResult(response) { StatusCode = response.Status };
    }
  }
}