namespace D9bolic.EntityFramework.DbContextScope.Implementations
{
    /// <summary>
    /// Db scope option
    /// </summary>
    public enum DbScopeOption : byte
    {
        /// <summary>
        /// Use it if it's not necessary to use any scope
        /// </summary>
        None = 0,

        /// <summary>
        /// Read only transaction option
        /// </summary>
        TransactionalReadOnly = 1,

        /// <summary>
        /// Transaction option
        /// </summary>
        Transactional = 2,
    }
}