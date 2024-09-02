using System.ComponentModel.DataAnnotations;

namespace GoldShop.Domain.Enums;

public enum BankStatus
{
    [Display(Name = "پرداخت موفق")]
    Successful = 0,
    [Display(Name = "در انتظار پرداخت")]
    Pending = 1,
    [Display(Name = "پرداخت ناموفق")]
    Cancel = 2,
 
}