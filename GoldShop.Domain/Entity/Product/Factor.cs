using System.ComponentModel.DataAnnotations.Schema;
using GoldShop.Domain.Common;
using GoldShop.Domain.Enums;

namespace GoldShop.Domain.Entity.Product;

public class Factor:BaseEntity
{
    public long UserId { get; set; }
    [ForeignKey("UserId")] public User.User User { get; set; } = new();
    public ICollection<ProductFactor> Products { get; set; } = default!;
    public string Address { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public Status Status { get; set; }
    public double GoldPerDay { get; set; } 
}