using System.ComponentModel.DataAnnotations.Schema;
using GoldShop.Domain.Common;
using GoldShop.Domain.Entity.User;
using GoldShop.Domain.Enums;

namespace GoldShop.Domain.Entity.Factor;

public class Factor:BaseEntity
{
    public long? UserAddressId { get; set; }
    [ForeignKey("UserAddressId")] public UserAddress? UserAddress { get; set; } 
    public long? UserId { get; set; }
    [ForeignKey("UserId")] public User.User? User { get; set; }
    public long? PostMethodId { get; set; }
    [ForeignKey("PostMethodId")] public PostMethod? PostMethod { get; set; }
    public string Desc { get; set; } = string.Empty;
    public string FactorCode { get; set; } = string.Empty;
    public string DiscountCode { get; set; } = string.Empty;
    public double DiscountAmount { get; set; }//discount code
    public double Amount { get; set; }//pure price
    public double GoldRate { get; set; }//pure price
    public DateTime InsertDate { get; set; }
    public Status Status { get; set; }
    public ICollection<FactorProduct> Products { get; set; } = default!;
}