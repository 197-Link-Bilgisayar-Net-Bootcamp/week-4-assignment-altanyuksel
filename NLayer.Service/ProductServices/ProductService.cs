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
    public async Task<Response<List<ProductDto>>> GetAllAsync() {
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
    public async Task<Response<ProductDto>> GetAsync(int id) {
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
    public async Task<Response<string>> CreateAllAsync(Category category, Product product, ProductFeature productFeature) {
      await categoryRepository.AddAsync(category);
      await productRepository.AddAsync(product);
      await productFeatureRepository.AddAsync(productFeature);

      await _unitOfWork.CommitAsyn();

      //ya da alttaki gibi de kullanılabilir
      //using (var transaction = _unitOfWork.BeginTransaction()) {
      //  await categoryRepository.Add(category);
      //  await productRepository.Add(product);
      //  await productFeatureRepository.Add(productFeature);
      //  transaction.Commit();
      //}

      return new Response<string>();
    }
  }
}