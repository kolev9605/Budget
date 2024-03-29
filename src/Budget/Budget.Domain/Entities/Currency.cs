﻿using Budget.Domain.Entities.Base;
using System.Collections.Generic;

namespace Budget.Domain.Entities;

public class Currency : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Abbreviation { get; set; } = null!;

    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
