using GoldShop.Domain.Entity.Factor;

namespace GoldShop.Views.Bank;

public class FactorModel
{
    public long? UserAddressId { get; set; }
    public long UserId { get; set; }
    public long? PostMethodId { get; set; }
    public string Desc { get; set; } = string.Empty;
    public string FactorCode { get; set; } = string.Empty;
    public string DiscountCode { get; set; } = string.Empty;
    public double DiscountAmount { get; set; }//discount code
    public double Amount { get; set; }//pure price
    public double GoldRate { get; set; }//pure price
    public DateTime InsertDate { get; set; }
    public List<FactorProductModel> Products { get; set; } = default!;
}