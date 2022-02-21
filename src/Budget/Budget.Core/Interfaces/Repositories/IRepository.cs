﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budget.Core.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> AllAsync();

        Task<T> GetByIdAsync(int id);
        
        Task<T> CreateAsync(T entity);
        
        Task<T> UpdateAsync(T entity);
        
        Task<T> DeleteAsync(int id);
    }
}
