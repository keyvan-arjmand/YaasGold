﻿namespace GoldShop.Application.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Username { get; }
}