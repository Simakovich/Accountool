using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using D9bolic.EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace D9bolic.EntityFramework.DbContextScope.Implementations
{
    /// <summary>
    /// This extension needs for saving changes without commit for DbContextScope nuget.
    /// We use reflection here because this nuget doesn't give us this functionality.
    /// But we can achieve it by using their methods.
    /// This library is fully tested but creator. But it can't be tested by unit tests.
    /// At first we are sure that these classes have fields and method which we need and it won't change if we don't update this nuget.
    /// If this functionality would have some problems it will be early detected on any QA phases.
    /// Also we have integration tests for this extension which helps us to recognize problems.
    /// </summary>
    public static class DbContextScopeExtension
    {
        private static readonly FieldInfo DisposedDbContextScopeField;
        private static readonly FieldInfo NestedDbContextScopeField;

        private static readonly MethodInfo GetAmbientScopeMethod;

        private static readonly FieldInfo TransactionsDbContextCollectionField;
        private static readonly FieldInfo ReadOnlyDbContextCollectionField;
        private static readonly FieldInfo DisposedDbContextCollectionField;
        private static readonly FieldInfo InitializedDbContextsDbContextCollectionField;

        // Static constructor for getting fields info one time and do not use reflection by any invocation
        // TODO: Review
#pragma warning disable S3963 // "static" fields should be initialized inline
#pragma warning disable S3011 // Used for our wrapping DbContextScope and DbContextCollection instances
        static DbContextScopeExtension()
        {
            GetAmbientScopeMethod =
                typeof(DbContextScope).GetMethod("GetAmbientScope", BindingFlags.NonPublic | BindingFlags.Static);

            var dbContextScopeFields = typeof(DbContextScope).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            DisposedDbContextScopeField = dbContextScopeFields.FirstOrDefault(x => x.Name == "_disposed");
            NestedDbContextScopeField = dbContextScopeFields.FirstOrDefault(x => x.Name == "_nested");

            var dbContextCollectionFields =
                typeof(DbContextCollection).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            TransactionsDbContextCollectionField =
                dbContextCollectionFields.FirstOrDefault(x => x.Name == "_transactions");
            ReadOnlyDbContextCollectionField = dbContextCollectionFields.FirstOrDefault(x => x.Name == "_readOnly");
            DisposedDbContextCollectionField = dbContextCollectionFields.FirstOrDefault(x => x.Name == "_disposed");
            InitializedDbContextsDbContextCollectionField =
                dbContextCollectionFields.FirstOrDefault(x => x.Name == "_initializedDbContexts");
        }
#pragma warning restore S3011 // Used for our wrapping DbContextScope and DbContextCollection instances
#pragma warning restore S3963 // "static" fields should be initialized inline

        /// <summary>
        /// Method for saving changes without commit, work also with nested scopes, but commit all changes made before in parent scopes
        /// </summary>
        /// <param name="dbContextScope">Current scope for saving changes without commit</param>
        /// <returns>Count of changes</returns>
        public static int SaveChangesWithoutCommitWithinParentScopes(this IDbContextScope dbContextScope)
        {
            Debug.WriteLine($"{Environment.StackTrace}{Environment.NewLine} was invoked withing parent scopes.");
            return SaveChangesWithoutCommit(dbContextScope, true);
        }

        /// <summary>
        /// Method for saving changes without commit it and make able to share changes in scope if parent scope would be created with isolation level
        /// </summary>
        /// <param name="dbContextScope">Current scope for saving changes without commit</param>
        /// <returns>Count of changes</returns>
        public static int SaveChangesWithoutCommit(this IDbContextScope dbContextScope)
        {
            return SaveChangesWithoutCommit(dbContextScope, false);
        }

        private static int SaveChangesWithoutCommit(IDbContextScope dbContextScope, bool includingNestedScopes)
        {
            var isProxy = dbContextScope.IsProxy();
            var changes = 0;
            if (!isProxy)
            {
                if (dbContextScope.IsDisposed())
                {
                    throw new ObjectDisposedException("DbContextScope");
                }

                if (!dbContextScope.DbContexts.IsWithinTransaction())
                {
                    var message = $"{nameof(SaveChangesWithoutCommit)} needs to be called within active transaction." +
                                  Environment.NewLine
                                  + "Usually it happens when service method is attributed with [Transactional] but no transaction isolation level is provided.";
                    throw new InvalidOperationException(message);
                }

                // Only save changes if we're not a nested scope. Otherwise, let the top-level scope
                // decide when the changes should be saved.
                if (includingNestedScopes || !dbContextScope.IsNested())
                {
                    changes = dbContextScope.SaveInternal();
                }
                else
                {
                    throw new InvalidOperationException(
                        $"It tries to save changes without commit from nested scope.{Environment.NewLine}"
                        + $"Suggested solutions:{Environment.NewLine}"
                        + "1) If you are able - do not use SaveChangesWithoutCommit method in the nested scope;"
                        + $"2) Otherwise you are able to use {nameof(SaveChangesWithoutCommitWithinParentScopes)}; It saves changes without commit to all contextes;"
                        + $"Stacktrace:{Environment.NewLine}"
                        + $"{Environment.StackTrace}");
                }
            }

            return changes;
        }

        /// <summary>
        /// Method for getting db collection of current scope
        /// </summary>
        /// <returns>Db context collection of current scope</returns>
        public static IDbContextCollection GetAmbientScopeDbCollection()
        {
            var scope = GetAmbientScopeMethod.Invoke(null, null);
            return (scope as IDbContextScope)?.DbContexts;
        }

        /// <summary>
        /// Indicates existing of parent scope
        /// </summary>
        /// <returns>True - it parent exists, false - if not</returns>
        public static bool HasParentScope()
        {
            return GetAmbientScopeMethod.Invoke(null, null) is DbContextScope;
        }

        /// <summary>
        /// Tries to get db context scope value
        /// </summary>
        /// <param name="dbContextCollection">Db context collection</param>
        /// <param name="context">Context if found</param>
        /// <returns>True - if there was value, false - if not</returns>
        internal static bool TryGetContext<TContext>(this IDbContextCollection dbContextCollection,
            out TContext context)
            where TContext : DbContext
        {
            var initializedContexts =
                (Dictionary<Type, DbContext>) InitializedDbContextsDbContextCollectionField.GetValue(
                    dbContextCollection);
            DbContext dbContext;
            var isFound = initializedContexts.TryGetValue(typeof(TContext), out dbContext);
            context = dbContext as TContext;
            return isFound;
        }

        /// <summary>
        /// Indetify is db context scope disposed
        /// </summary>
        /// <param name="dbContextScope">Context scope</param>
        /// <returns>True - if disposed, false - if not</returns>
        internal static bool IsDisposed(this IDbContextScope dbContextScope)
        {
            var value = DisposedDbContextScopeField.GetValue(dbContextScope);
            return (bool) value;
        }

        /// <summary>
        /// Indetify is db context collection disposed
        /// </summary>
        /// <param name="dbContextCollection">Db Context collcetion</param>
        /// <returns>True - if disposed, false - if not</returns>
        internal static bool IsDisposed(this IDbContextCollection dbContextCollection)
        {
            var value = DisposedDbContextCollectionField.GetValue(dbContextCollection);
            return (bool) value;
        }

        /// <summary>
        /// Method for getting db collection of specified scope
        /// </summary>
        /// <returns>Db context collection of specified scope</returns>
        internal static IDbContextCollection GetDbCollectionFromDbScope(object dbScope)
        {
            var dbContextScope = dbScope as IDbContextScope;
            var dbContextReadOnlyScope = dbScope as IDbContextReadOnlyScope;
            return dbContextScope?.DbContexts ?? dbContextReadOnlyScope?.DbContexts;
        }

        private static int SaveInternal(this IDbContextScope dbContextScope)
        {
            var dbCollection = dbContextScope.DbContexts;
            var changes = 0;

            ExceptionDispatchInfo lastError = null;

            var dbContexts = dbCollection.ToDbContexts();
            var readOnly = dbCollection.IsReadOnly();
            foreach (var dbContext in dbContexts)
            {
                try
                {
                    if (!readOnly)
                    {
                        changes += dbContext.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    lastError = ExceptionDispatchInfo.Capture(e);
                }
            }

            lastError?.Throw(); // Re-throw while maintaining the exception's original stack track

            return changes;
        }

        private static bool IsNested(this IDbContextScope dbContextScope)
        {
            var value = NestedDbContextScopeField.GetValue(dbContextScope);
            return (bool) value;
        }

        private static bool IsWithinTransaction(this IDbContextCollection dbContextCollection)
        {
            var value = TransactionsDbContextCollectionField.GetValue(dbContextCollection);
            var transactions = (Dictionary<DbContext, IDbContextTransaction>) value;
            return transactions.Any();
        }

        private static bool IsReadOnly(this IDbContextCollection dbContextCollection)
        {
            var value = ReadOnlyDbContextCollectionField.GetValue(dbContextCollection);
            return (bool) value;
        }

        private static IEnumerable<DbContext> ToDbContexts(this IDbContextCollection dbContextCollection)
        {
            var value = InitializedDbContextsDbContextCollectionField.GetValue(dbContextCollection);
            var dbContextDictionary = value as Dictionary<Type, DbContext>;
            return dbContextDictionary?.Values;
        }

        private static bool IsProxy(this object obj)
        {
            var type = obj.GetType();
            var isProxy = type.FullName.StartsWith("Castle.Proxies") && type.FullName.EndsWith("Proxy");
            return isProxy;
        }
    }
}