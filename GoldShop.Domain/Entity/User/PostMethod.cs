using GoldShop.Domain.Common;

namespace GoldShop.Domain.Entity.User;

public class PostMethod:BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public long Price { get; set; }
    public bool IsFee { get; set; }
}