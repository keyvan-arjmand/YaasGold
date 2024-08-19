using GoldShop.Domain.Entity.Product;

namespace GoldShop.Models;

public class CheckOutDto
{
    public long Id { get; set; }
    public double Weight { get; set; }
    public string? Size { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;

    public double WagesAmount { get; set; } //اجرت 
    public double WagesPercentage { get; set; } //اجرت 
    public GoldPrice GoldPrice { get; set; } = default!;
}