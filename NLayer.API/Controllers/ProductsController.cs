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
      var response = await _productService.GetAllAsyncDto();

      return new ObjectResult(response) { StatusCode = response.Status };
    }
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id) {
      var response = await _productService.GetAsyncDto(id);

      return new ObjectResult(response) { StatusCode = response.Status };
    }
    [Authorize]
    [HttpGet("[action]")]
    public async Task<IActionResult> GetAllProduct() {
      var products = await _productService.GetAllAsync();
      return new ObjectResult(products);
    }
    [HttpPost()]
    public async Task<IActionResult> CreateProduct([FromBody] List<Product> listProduct) {
      var response = await _productService.Update(listProduct);
      return new ObjectResult(response) { StatusCode = response.Status };
    }
    [Authorize]
    [HttpPost("[Action]")]
    public async ValueTask<IActionResult> AddProductAsync([FromBody] List<ProductDto> listProd) {
      var response = await _productService.CreateProductsAsync(listProd);
      return new ObjectResult(response) { StatusCode = response.Status };
    }
  }
}