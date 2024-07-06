using System.ComponentModel.DataAnnotations;

namespace GoldShop.Domain.Common;

public class BaseEntity
{

    [Key] public long Id { get; set; }
    public bool IsDelete { get; set; }= false;
}
