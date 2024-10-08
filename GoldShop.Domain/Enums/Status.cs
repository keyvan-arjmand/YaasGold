﻿using System.ComponentModel.DataAnnotations;

namespace GoldShop.Domain.Enums;

public enum Status
{
    [Display(Name = "ارسال شده")]
    Accepted = 0,
    [Display(Name = "رد شده")]
    Rejected = 1,
    [Display(Name = "در حال پردازش")]
    Pending=2,
    [Display(Name = "مرجوعی")]
    Returned = 3
}