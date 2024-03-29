﻿using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Shared;

public class UserChangePassword
{
    [Required, StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Compare(nameof(Password), ErrorMessage = "The password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}