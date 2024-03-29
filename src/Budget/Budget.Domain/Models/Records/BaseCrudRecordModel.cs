﻿using Budget.Domain.Entities;
using System;

namespace Budget.Domain.Models.Records;

public abstract class BaseCrudRecordModel
{
    public string Note { get; set; } = null!;

    public decimal Amount { get; set; }

    public int AccountId { get; set; }

    public int CategoryId { get; set; }

    public int PaymentTypeId { get; set; }

    public RecordType RecordType { get; set; }

    public DateTime RecordDate { get; set; }

    public int? FromAccountId { get; set; }
}
