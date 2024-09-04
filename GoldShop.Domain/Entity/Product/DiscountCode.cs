using GoldShop.Domain.Common;

namespace GoldShop.Domain.Entity.Product;

public class DiscountCode:BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public long Amount { get; set; }
    public int Count { get; set; }
    public bool IsActive { get; set; }
}