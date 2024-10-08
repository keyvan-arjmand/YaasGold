﻿using GoldShop.Domain.Common;

namespace GoldShop.Domain.Entity.User;

public class State : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public ICollection<City> Cities { get; set; } = default!;//اشتراک
}