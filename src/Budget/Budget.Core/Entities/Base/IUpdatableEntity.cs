using System;

namespace Budget.Core.Entities.Base
{
    public interface IUpdatableEntity
    {
        public DateTime UpdatedAt { get; set; }
    }
}
