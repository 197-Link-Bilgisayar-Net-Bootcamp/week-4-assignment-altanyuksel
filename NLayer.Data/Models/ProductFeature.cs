using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Data.Models {
  public class ProductFeature {
    public int Id { get; set; } 
    public int Width { get; set; }
    public int Height { get; set; }
    public string color { get; set; }
    public Product Product { get; set; } 
  }
}
