using System.ComponentModel.DataAnnotations.Schema;
using GoldShop.Domain.Common;

namespace GoldShop.Domain.Entity.User;

public class UserAddress:BaseEntity
{
    public long UserId { get; set; }
    public long CityId { get; set; }
    [ForeignKey("CityId")] public City City { get; set; } = default!;
    public string Address { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string PostCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}