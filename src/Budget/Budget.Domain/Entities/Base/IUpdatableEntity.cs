using System;

namespace Budget.Domain.Entities.Base
{
    public interface IUpdatableEntity
    {
        public DateTime UpdatedAt { get; set; }
    }
}
