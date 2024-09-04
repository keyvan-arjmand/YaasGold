using Microsoft.AspNetCore.Http;

namespace GoldShop.Application.Dtos.Products;

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public IFormFile ImageBanner { get; set; } = default!;
    public IFormFile ImageThumb { get; set; } = default;
    public IFormFile Image { get; set; } = default;
    public string Weight { get; set; } = string.Empty;
    public double Wages { get; set; }//اجرت 
    public double WagesPercentage { get; set; } 
    public long Inventory { get; set; }
    public long CategoryId { get; set; }
    public bool IsSpec { get; set; }
}