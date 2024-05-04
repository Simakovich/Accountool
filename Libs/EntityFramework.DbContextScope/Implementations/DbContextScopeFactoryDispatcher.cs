using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using D9bolic.EntityFramework.DbContextScope.Enums;
using D9bolic.EntityFramework.DbContextScope.Interfaces;

namespace D9bolic.EntityFramework.DbContextScope.Implementations
{
    /// <inheritdoc cref="IDbContextScopeFactory"/>
    public class DbContextScopeFactoryDispatcher : IDbContextScopeFactory, IDbContextScopeContainer
    {
        private static readonly object Lock = new object();
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly List<DbScopeInfo> _dbScopeInfos = new List<DbScopeInfo>();

        public DbContextScopeFactoryDispatcher(DbContextScopeFactory dbContextScopeFactory)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
        }

        /// <inheritdoc />
        DbScopeOption IDbContextScopeContainer.GetDbScopeOption<TContext>(TContext currentContext)
        {
            lock (Lock)
            {
                var disposedScopes = _dbScopeInfos.Where(x => x.IsDisposed).ToList();
                if (disposedScopes.Any())
                {
                    disposedScopes.ForEach(x => _dbScopeInfos.Remove(x));
                }

                foreach (var dbScopeInfo in _dbScopeInfos)
                {
                    TContext context;
                    var isFound = dbScopeInfo.DbCollection.TryGetContext(out context);
                    if (isFound && ReferenceEquals(context, currentContext))
                    {
                        return dbScopeInfo.DbScopeOption;
                    }
                }
            }

            throw new ArgumentException("Db context scope wasn't found");
        }

        /// <inheritdoc />
        public IDbContextScope Create(DbContextScopeOption joiningOption = DbContextScopeOption.JoinExisting)
        {
            var scope = _dbContextScopeFactory.Create(joiningOption);
            AddScope(new DbScopeInfo(scope, DbScopeOption.Transactional));
            return scope;
        }

        /// <inheritdoc />
        public IDbContextReadOnlyScope CreateReadOnly(
            DbContextScopeOption joiningOption = DbContextScopeOption.JoinExisting)
        {
            var scope = _dbContextScopeFactory.CreateReadOnly(joiningOption);
            AddScope(new DbScopeInfo(scope, DbScopeOption.TransactionalReadOnly));
            return scope;
        }

        /// <inheritdoc />
        public IDbContextScope CreateWithTransaction(IsolationLevel isolationLevel)
        {
            var scope = _dbContextScopeFactory.CreateWithTransaction(isolationLevel);
            AddScope(new DbScopeInfo(scope, DbScopeOption.Transactional));
            return scope;
        }

        /// <inheritdoc />
        public IDbContextReadOnlyScope CreateReadOnlyWithTransaction(IsolationLevel isolationLevel)
        {
            var scope = _dbContextScopeFactory.CreateReadOnlyWithTransaction(isolationLevel);
            AddScope(new DbScopeInfo(scope, DbScopeOption.TransactionalReadOnly));
            return scope;
        }

        /// <inheritdoc />
        public IDisposable SuppressAmbientContext()
        {
            throw new NotSupportedException("No need to implement this method. No usages expected.");
        }

        private void AddScope(DbScopeInfo dbScopeInfo)
        {
            lock (Lock)
            {
                _dbScopeInfos.Add(dbScopeInfo);
            }
        }

        private class DbScopeInfo
        {
            private readonly IDbContextCollection _dbCollection;

            public DbScopeInfo(object dbScope, DbScopeOption dbScopeOption)
            {
                _dbCollection = DbContextScopeExtension.GetDbCollectionFromDbScope(dbScope);
                DbScopeOption = dbScopeOption;
            }

            public DbScopeOption DbScopeOption { get; }

            public bool IsDisposed => _dbCollection.IsDisposed();

            public IDbContextCollection DbCollection => _dbCollection;
        }
    }
}