using System.ComponentModel.DataAnnotations.Schema;
using GoldShop.Domain.Common;

namespace GoldShop.Domain.Entity.Factor;

public class FactorProduct:BaseEntity
{
    public long? FactorId { get; set; }
    [ForeignKey("FactorId")] public Factor? Factor { get; set; }
    public long? ProductId { get; set; }
    [ForeignKey("ProductId")] public Product.Product? ProductColor { get; set; }
    public double Weight { get; set; }
    public int Count { get; set; }
}