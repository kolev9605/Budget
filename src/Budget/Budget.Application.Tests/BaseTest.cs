using Budget.Application.Mappings;
using Budget.Domain.Entities;
using Budget.Persistance;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;
using System.Reflection;

namespace Budget.Application.Tests
{
    public abstract class BaseTest : IDisposable
    {
        protected DbConnection _connection;
        protected DbContextOptions<BudgetDbContext> _contextOptions;

        public BaseTest()
        {
            GetMapper();
        }

        protected Mapper GetMapper()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(RecordMappingConfiguration).Assembly);
            config.Scan(typeof(Account).Assembly);
            return new Mapper(config);
        }

        protected BudgetDbContext CreateContext() => new BudgetDbContext(_contextOptions);

        public void Dispose() => _connection.Dispose();
    }
}
