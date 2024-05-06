using Accountool.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Accountool.Data
{
    public interface IDbContextScopeFactory
    {
        DbContextScope CreateReadOnly();
    }

    public class DbContextScopeFactory : IDbContextScopeFactory
    {
        public DbContextScope CreateReadOnly()
        {
            return new DbContextScope(new ApplicationDbContext(), readOnly: true);
        }
    }

    public class DbContextScope : IDisposable
    {
        private bool _disposed;
        private readonly DbContext _dbContext;

        public DbContextScope(ApplicationDbContext dbContext, bool readOnly)
        {
            _dbContext = dbContext;
            _dbContext.ChangeTracker.QueryTrackingBehavior = readOnly
                ? QueryTrackingBehavior.NoTracking
                : QueryTrackingBehavior.TrackAll;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _dbContext.Dispose();
            }
        }
    }
}
