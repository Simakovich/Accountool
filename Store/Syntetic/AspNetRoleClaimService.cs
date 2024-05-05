using System.Data;
using Accountool.DataAccess;
using Accountool.Models;
using D9bolic.EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Accountool.Services;
public partial interface IAspNetRoleClaimService
{
    Task<IEnumerable<AspNetRoleClaim>> GetAll();
    Task<AspNetRoleClaim?> GetById(int id);
    Task<int> Create(AspNetRoleClaim entity);
    Task Update(AspNetRoleClaim entity);
    Task Delete(int entityId);
    Task<IEnumerable<AspNetRoleClaim>> GetForRole(int RoleId);
}

public partial class AspNetRoleClaimService : IAspNetRoleClaimService
{
    private readonly IRepository<AspNetRoleClaim> _repository;
    private readonly IDbContextScopeFactory _dbContextScopeFactory;
    public AspNetRoleClaimService(IRepository<AspNetRoleClaim> repository, IDbContextScopeFactory dbContextScopeFactory)
    {
        _repository = repository;
        _dbContextScopeFactory = dbContextScopeFactory;
    }

    public async Task<IEnumerable<AspNetRoleClaim>> GetAll()
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().ToArrayAsync();
    }

    public async Task<AspNetRoleClaim?> GetById(int id)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.GetById(id);
    }

    public async Task<int> Create(AspNetRoleClaim entity)
    {
        using var scope = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted);
        _repository.Add(entity);
        await scope.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Update(AspNetRoleClaim entity)
    {
        using var scope = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted);
        _repository.Update(entity);
        await scope.SaveChangesAsync();
    }

    public async Task Delete(int entityId)
    {
        using var scope = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted);
        var entity = await _repository.GetById(entityId);
        _repository.Delete(entity);
        await scope.SaveChangesAsync();
    }

    public async Task<IEnumerable<AspNetRoleClaim>> GetForRole(int RoleId)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().Where(entity => entity.RoleId == RoleId).ToArrayAsync();
    }
}