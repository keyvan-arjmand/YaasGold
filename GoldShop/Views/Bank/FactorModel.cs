using GoldShop.Domain.Entity.Factor;

namespace GoldShop.Views.Bank;

public class FactorModel
{
    public long StateId { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string PostCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public DateTime InsertDate { get; set; }
}