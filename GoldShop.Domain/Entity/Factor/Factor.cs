using System.ComponentModel.DataAnnotations.Schema;
using GoldShop.Domain.Common;
using GoldShop.Domain.Entity.User;
using GoldShop.Domain.Enums;

namespace GoldShop.Domain.Entity.Factor;
public class Factor : BaseEntity
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
    public long DiscountAmount { get; set; } //discount code
    public long Amount { get; set; } //pure price
    public long GoldRate { get; set; } //pure price
    public DateTime InsertDate { get; set; }
    public Status StatusEdit { get; set; }
    public BankStatus BankStatus { get; set; }
    public ICollection<FactorProduct> Products { get; set; } = default!;
    public string? Token { get; set; }
    public int? TerminalNo { get; set; }
    public long? RRN { get; set; }
    public string? StatusDescription { get; set; }  // نام این ستون تغییر داده شده است
    public string? HashCardNumber { get; set; }
    public string? TspToken { get; set; }
    public string? DescBank { get; set; }
}