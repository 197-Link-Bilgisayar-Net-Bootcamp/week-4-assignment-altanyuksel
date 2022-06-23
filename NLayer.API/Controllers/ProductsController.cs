using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLayer.Data.Models;
using NLayer.Service.Dtos;
using NLayer.Service.ProductServices;

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
      var response = await _productService.GetAllDtoAsync();
      return new ObjectResult(response) { StatusCode = response.Status };
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id) {
      var response = await _productService.GetDtoAsync(id);
      return new ObjectResult(response) { StatusCode = response.Status };
    }
    [Authorize]
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllProductAsync() {
      var products = await _productService.GetAllAsync();
      return new ObjectResult(products);
    }
    [Authorize]
    [HttpPost("[Action]")]
    public async ValueTask<IActionResult> CreateProductAsync([FromBody] List<ProductDto> listProd) {
      var response = await _productService.CreateProductsAsync(listProd);
      return new ObjectResult(response) { StatusCode = response.Status };
    }
  }
}