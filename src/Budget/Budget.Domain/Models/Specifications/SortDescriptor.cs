using System.Linq.Expressions;
using System;

namespace Budget.Domain.Models.Specifications
{
    public class SortDescriptor
    {
        public SortDescriptor(string property, SortDirection direction)
        {
            Property = property;
            Direction = direction;
        }

        public string Property { get; set; }

        public SortDirection Direction { get; set; }
    }
}
