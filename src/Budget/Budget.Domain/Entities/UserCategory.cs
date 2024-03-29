﻿namespace Budget.Domain.Entities;

public class UserCategory
{
    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;
}
