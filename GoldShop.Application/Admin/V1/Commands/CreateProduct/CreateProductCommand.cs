using MediatR;
using Microsoft.AspNetCore.Http;

namespace GoldShop.Application.Admin.V1.Commands.CreateProduct;

public class CreateProductCommand : IRequest
{
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string ImageBanner { get; set; } = string.Empty;
    public string ImageThumb { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public string Weight { get; set; } //وزن 
    public double Wages { get; set; } //اجرت 
    public double WagesPercentage { get; set; } //اجرت 
    public long Inventory { get; set; }
    public long CategoryId { get; set; }
    public bool IsSpec { get; set; }

}