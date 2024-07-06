using System.ComponentModel.DataAnnotations.Schema;
using GoldShop.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace GoldShop.Domain.Entity.Product;

public class ProductFactor:BaseEntity
{
    public long ProductId { get; set; }
    [ForeignKey(nameof(ProductId))] public Product Product { get; set; } = default!;
    public long FactorId { get; set; }
    [DeleteBehavior(DeleteBehavior.ClientSetNull)]
    [ForeignKey(nameof(FactorId))] public Factor Factor { get; set; } = default!;

}