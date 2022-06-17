using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Data.Models {
  public class Product {
    public int Id { get; set; } 
    public string Name { get; set; }
    public decimal Price { get; set; }
    public decimal Stock { get; set; }
    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public ProductFeature ProductFeature { get; set; }
  }
}
