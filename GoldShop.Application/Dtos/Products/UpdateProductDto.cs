using Microsoft.AspNetCore.Http;

namespace GoldShop.Application.Dtos.Products;

public class UpdateProductDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public IFormFile ImageBanner { get; set; } = default!;
    public string ImageBannerName { get; set; } = default!;
    public IFormFile ImageThumb { get; set; } = default!;
    public string ImageThumbName { get; set; } = default!;
    public IFormFile Image { get; set; } = default!;
    public string ImageName { get; set; } = default!;
    public string Weight { get; set; } = string.Empty;
    public double Wages { get; set; }//اجرت 
    public double WagesPercentage { get; set; } 
    public long Inventory { get; set; }
    public int CategoryId { get; set; }
    public bool IsSpec { get; set; }
}