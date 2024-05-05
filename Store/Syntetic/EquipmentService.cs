using System.Data;
using Accountool.DataAccess;
using Accountool.Models;
using D9bolic.EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Accountool.Services;
public partial interface IEquipmentService
{
    Task<IEnumerable<Equipment>> GetAll();
    Task<Equipment?> GetById(int id);
    Task<int> Create(Equipment entity);
    Task Update(Equipment entity);
    Task Delete(int entityId);
    Task<IEnumerable<Equipment>> GetForKiosk(int KioskId);
}

public partial class EquipmentService : IEquipmentService
{
    private readonly IRepository<Equipment> _repository;
    private readonly IDbContextScopeFactory _dbContextScopeFactory;
    public EquipmentService(IRepository<Equipment> repository, IDbContextScopeFactory dbContextScopeFactory)
    {
        _repository = repository;
        _dbContextScopeFactory = dbContextScopeFactory;
    }

    public async Task<IEnumerable<Equipment>> GetAll()
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().ToArrayAsync();
    }

    public async Task<Equipment?> GetById(int id)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.GetById(id);
    }

    public async Task<int> Create(Equipment entity)
    {
        using var scope = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted);
        _repository.Add(entity);
        await scope.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Update(Equipment entity)
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

    public async Task<IEnumerable<Equipment>> GetForKiosk(int KioskId)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().Where(entity => entity.KioskId == KioskId).ToArrayAsync();
    }
}