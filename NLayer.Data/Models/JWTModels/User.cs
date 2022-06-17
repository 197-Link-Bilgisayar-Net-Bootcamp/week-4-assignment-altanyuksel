using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NLayer.Data.Models.JWTModels {
  public class User {
    [Key]
    [JsonIgnore]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string RefreshToken { get; set; }
    [JsonIgnore]
    public DateTime RefreshTokenExpiryTime { get; set; }
  }
}