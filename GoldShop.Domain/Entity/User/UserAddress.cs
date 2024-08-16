using System.ComponentModel.DataAnnotations.Schema;
using GoldShop.Domain.Common;

namespace GoldShop.Domain.Entity.User;

public class UserAddress:BaseEntity
{
    public long UserId { get; set; }
    public long StateId { get; set; }
    [ForeignKey("StateId")] public State? State { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string PostCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}