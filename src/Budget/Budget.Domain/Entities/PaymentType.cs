﻿using Budget.Domain.Entities.Base;

namespace Budget.Domain.Entities;

public class PaymentType : BaseEntity
{
    public string Name { get; set; } = null!;

    public ICollection<Record> Records { get; set; } = new List<Record>();
}
