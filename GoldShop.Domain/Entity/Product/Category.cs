using GoldShop.Domain.Common;

namespace GoldShop.Domain.Entity.Product;

public class Category:BaseEntity
{
    public string Name { get; set; }=string.Empty;
}