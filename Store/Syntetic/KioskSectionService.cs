using System.Data;
using Accountool.DataAccess;
using Accountool.Models;
using D9bolic.EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Accountool.Services;
public partial interface IKioskSectionService
{
    Task<IEnumerable<KioskSection>> GetAll();
    Task<KioskSection?> GetById(int id);
    Task<int> Create(KioskSection entity);
    Task Update(KioskSection entity);
    Task Delete(int entityId);
}

public partial class KioskSectionService : IKioskSectionService
{
    private readonly IRepository<KioskSection> _repository;
    private readonly IDbContextScopeFactory _dbContextScopeFactory;
    public KioskSectionService(IRepository<KioskSection> repository, IDbContextScopeFactory dbContextScopeFactory)
    {
        _repository = repository;
        _dbContextScopeFactory = dbContextScopeFactory;
    }

    public async Task<IEnumerable<KioskSection>> GetAll()
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().ToArrayAsync();
    }

    public async Task<KioskSection?> GetById(int id)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.GetById(id);
    }

    public async Task<int> Create(KioskSection entity)
    {
        using var scope = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted);
        _repository.Add(entity);
        await scope.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Update(KioskSection entity)
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