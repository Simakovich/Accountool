using System.Data;
using Accountool.DataAccess;
using Accountool.Models;
using D9bolic.EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Accountool.Services;
public partial interface IIndicationService
{
    Task<IEnumerable<Indication>> GetAll();
    Task<Indication?> GetById(int id);
    Task<int> Create(Indication entity);
    Task Update(Indication entity);
    Task Delete(int entityId);
    Task<IEnumerable<Indication>> GetForSchetchik(int SchetchikId);
}

public partial class IndicationService : IIndicationService
{
    private readonly IRepository<Indication> _repository;
    private readonly IDbContextScopeFactory _dbContextScopeFactory;
    public IndicationService(IRepository<Indication> repository, IDbContextScopeFactory dbContextScopeFactory)
    {
        _repository = repository;
        _dbContextScopeFactory = dbContextScopeFactory;
    }

    public async Task<IEnumerable<Indication>> GetAll()
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().ToArrayAsync();
    }

    public async Task<Indication?> GetById(int id)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.GetById(id);
    }

    public async Task<int> Create(Indication entity)
    {
        using var scope = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted);
        _repository.Add(entity);
        await scope.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Update(Indication entity)
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

    public async Task<IEnumerable<Indication>> GetForSchetchik(int SchetchikId)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().Where(entity => entity.SchetchikId == SchetchikId).ToArrayAsync();
    }
}