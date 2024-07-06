using System.ComponentModel.DataAnnotations.Schema;
using GoldShop.Domain.Common;

namespace GoldShop.Domain.Entity.User;

public class City : BaseEntity
{
    public long StateId { get; set; }
    [ForeignKey("StateId")] public State State { get; set; } = new();

    public string Name { get; set; } = string.Empty;


}