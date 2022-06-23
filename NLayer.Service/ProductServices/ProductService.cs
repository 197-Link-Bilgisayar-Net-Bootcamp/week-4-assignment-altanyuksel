using Microsoft.EntityFrameworkCore;
using NLayer.Data;
using NLayer.Data.Models;
using NLayer.Data.Repositories;
using NLayer.Service.Dtos;
using NLayer.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.ProductServices {
  public class ProductService {
    private readonly GenericRepository<Product> productRepository;
    private readonly GenericRepository<Category> categoryRepository;
    private readonly GenericRepository<ProductFeature> productFeatureRepository;
    private readonly UnitOfWork _unitOfWork;
    public ProductService(GenericRepository<Product> productRepository, GenericRepository<Category> categoryRepository, GenericRepository<ProductFeature> productFeatureRepository, UnitOfWork unitOfWork) {
      this.productRepository = productRepository;
      this.categoryRepository = categoryRepository;
      this.productFeatureRepository = productFeatureRepository;
      _unitOfWork = unitOfWork;
    }
    public async Task<Response<List<ProductDto>>> GetAllDtoAsync() {
      var prodList = await productRepository.GetAllAsync();
      var prodDtos = prodList.Select(p => new ProductDto() {
        Id = p.Id,
        Name = p.Name,
        Price = p.Price,
        CategoryId = p.CategoryId
      }).ToList();
      if (!prodDtos.Any()) {
        return new Response<List<ProductDto>> {
          Data = null,
          Errors = new List<string> { "Ürünler bulunamadı." },
          Status = 404
        };
      }
      return new Response<List<ProductDto>> {
        Data = prodDtos,
        Errors = null,
        Status = 200
      };
    }
    public async Task<Response<ProductDto>> GetDtoAsync(int id) {
      var prodList = await productRepository.GetAsync(id);
      if (prodList == null) {
        return new Response<ProductDto> {
          Data = null,
          Errors = new List<string> { "Ürünler bulunamadı." },
          Status = 404
        };
      }
      var prodDtos = new ProductDto() {
        Id = prodList.Id,
        Name = prodList.Name,
        Price = prodList.Price,
        CategoryId = prodList.CategoryId
      };
      return new Response<ProductDto> {
        Data = prodDtos,
        Errors = null,
        Status = 200
      };
    }
    public async Task<Response<List<Product>>> GetAllAsync() {
      var prodList = await productRepository.GetAllAsync();
      var prodDtos = prodList.Select(p => new ProductDto() {
        Id = p.Id,
        Name = p.Name,
        Price = p.Price,
        CategoryId = p.CategoryId
      }).ToList();
      if (!prodList.Any()) {
        return new Response<List<Product>> {
          Data = null,
          Errors = new List<string> { "Ürünler bulunamadı." },
          Status = 404
        };
      }
      return new Response<List<Product>> {
        Data = prodList,
        Errors = null,
        Status = 200
      };
    }
    public async Task<Response<Product>> GetAsync(int id) {
      var prodList = await productRepository.GetAsync(id);
      if (prodList == null) {
        return new Response<Product> {
          Data = null,
          Errors = new List<string> { "Ürünler bulunamadı." },
          Status = 404
        };
      }
      return new Response<Product> {
        Data = prodList,
        Errors = null,
        Status = 200
      };
    }
    public async Task<Response<List<Product>>> UpdateAsync(List<Product> listProduct) {
      await productRepository.UpdateAysnc(listProduct);
      await _unitOfWork.CommitAsyn();
      return new Response<List<Product>> {
        Data = listProduct,
        Errors = null,
        Status = 200
      };
    }
    public async ValueTask<Response<List<Product>>> CreateProductsAsync(List<ProductDto> productDtos) {
      var listProduct = new List<Product>();
      var listError = new List<string>();
      foreach (var product in productDtos) {
        var category = await categoryRepository.GetItemsAsync(c => c.Id == product.CategoryId);
        if (category.Count > 0) {
          listProduct.Add(new Product() {
            CategoryId = product.CategoryId,
            Name = product.Name,
            Price = product.Price,
            ProductFeature = new ProductFeature() { Height = 0, Width = 0 },
            Stock = product.Stock,
            Category = category[0]
          });
        }
        else { listError.Add($"'{product.Name}' ürünü için categori bulunamadı."); }
      }
      var res = new Response<List<Product>>() { 
        Data = listProduct,
        Errors = listError,
        Status = listError.Count > 0 ? 400 : 200
    };
      foreach (var item in listProduct) {
        await productRepository.AddAsync(item);
      }
      return res;
    }
  }
}