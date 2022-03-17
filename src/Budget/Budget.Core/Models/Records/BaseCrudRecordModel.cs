﻿using Budget.Core.Entities;
using System;

namespace Budget.Core.Models.Records
{
    public abstract class BaseCrudRecordModel
    {
        public string Note { get; set; }

        public decimal Amount { get; set; }

        public int AccountId { get; set; }

        public int CategoryId { get; set; }

        public int PaymentTypeId { get; set; }

        public RecordType RecordType { get; set; }

        public DateTime RecordDate { get; set; }
    }
}
