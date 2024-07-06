using System.ComponentModel.DataAnnotations.Schema;
using GoldShop.Domain.Common;

namespace GoldShop.Domain.Entity.Product;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string ImageBanner { get; set; } = string.Empty;
    public string ImageThumb { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public double Weight { get; set; } //وزن 
    public double Wages { get; set; } //اجرت 
    public long Inventory { get; set; }
    public bool IsSpecial { get; set; }
    public DateTime InsertDate { get; set; }
    public long CategoryId { get; set; }
    [ForeignKey("CategoryId")] public Category Category { get; set; } = default!;
    public long GoldPriceId { get; set; }
    [ForeignKey("GoldPriceId")] public GoldPrice GoldPrice { get; set; } = default!;
}