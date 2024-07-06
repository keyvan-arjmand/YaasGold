using GoldShop.Domain.Common;
using GoldShop.Domain.Enums;

namespace GoldShop.Domain.Entity.Product;

public class GoldPrice : BaseEntity
{
    public double PricePerGram { get; set; }
    public string GenderType { get; set; } = string.Empty; //نوع جنس
    public PriceType PriceType { get; set; }
}