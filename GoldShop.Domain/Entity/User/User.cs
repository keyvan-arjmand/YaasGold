using System.ComponentModel.DataAnnotations.Schema;
using GoldShop.Domain.Common;
using GoldShop.Domain.Entity.Product;
using Microsoft.AspNetCore.Identity;

namespace GoldShop.Domain.Entity.User;

public class User : IdentityUser<long>
{
    public string Name { get; set; } = string.Empty;
    public string Family { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;
    public long CityId { get; set; }
    [ForeignKey("CityId")] public City? City { get; set; } 
    public string ConfirmCode { get; set; } = string.Empty;
    public string NationalCode { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime InsertDate { get; set; } = DateTime.Now;
    public DateTime ConfirmCodeExpireTime { get; set; }
    public string Address { get; set; } = string.Empty;

    public ICollection<Factor.Factor> Factors { get; set; } = default!;
    
}