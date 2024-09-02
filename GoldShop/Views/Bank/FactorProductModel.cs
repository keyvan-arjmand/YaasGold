using GoldShop.Domain.Entity.Factor;
using GoldShop.Domain.Entity.Product;

namespace GoldShop.Views.Bank;

public class FactorProductModel
{
    public long? ProductId { get; set; }
    public double Weight { get; set; }
    public string? Size { get; set; }
}