using System.Data;
using Accountool.DataAccess;
using Accountool.Models;
using D9bolic.EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Accountool.Services;
public partial interface IOrgNameService
{
    Task<IEnumerable<OrgName>> GetAll();
    Task<OrgName?> GetById(int id);
    Task<int> Create(OrgName entity);
    Task Update(OrgName entity);
    Task Delete(int entityId);
}

public partial class OrgNameService : IOrgNameService
{
    private readonly IRepository<OrgName> _repository;
    private readonly IDbContextScopeFactory _dbContextScopeFactory;
    public OrgNameService(IRepository<OrgName> repository, IDbContextScopeFactory dbContextScopeFactory)
    {
        _repository = repository;
        _dbContextScopeFactory = dbContextScopeFactory;
    }

    public async Task<IEnumerable<OrgName>> GetAll()
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().ToArrayAsync();
    }

    public async Task<OrgName?> GetById(int id)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.GetById(id);
    }

    public async Task<int> Create(OrgName entity)
    {
        using var scope = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted);
        _repository.Add(entity);
        await scope.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Update(OrgName entity)
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
}