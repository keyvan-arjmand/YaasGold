using GoldShop.Domain.Common;
using GoldShop.Domain.Enums;

namespace GoldShop.Domain.Entity.Page;

public class ContactUs:BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public Subject Subject { get; set; }
    public DateTime InsertDate { get; set; }
}