using System.Data;
using Accountool.DataAccess;
using Accountool.Models;
using D9bolic.EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Accountool.Services;
public partial interface ISchetchikService
{
    Task<IEnumerable<Schetchik>> GetAll();
    Task<Schetchik?> GetById(int id);
    Task<int> Create(Schetchik entity);
    Task Update(Schetchik entity);
    Task Delete(int entityId);
    Task<IEnumerable<Schetchik>> GetForKiosk(int KioskId);
}

public partial class SchetchikService : ISchetchikService
{
    private readonly IRepository<Schetchik> _repository;
    private readonly IDbContextScopeFactory _dbContextScopeFactory;
    public SchetchikService(IRepository<Schetchik> repository, IDbContextScopeFactory dbContextScopeFactory)
    {
        _repository = repository;
        _dbContextScopeFactory = dbContextScopeFactory;
    }

    public async Task<IEnumerable<Schetchik>> GetAll()
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().ToArrayAsync();
    }

    public async Task<Schetchik?> GetById(int id)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.GetById(id);
    }

    public async Task<int> Create(Schetchik entity)
    {
        using var scope = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted);
        _repository.Add(entity);
        await scope.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Update(Schetchik entity)
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

    public async Task<IEnumerable<Schetchik>> GetForKiosk(int KioskId)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().Where(entity => entity.KioskId == KioskId).ToArrayAsync();
    }
}