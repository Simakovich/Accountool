using System.Data;
using Accountool.DataAccess;
using Accountool.Models;
using D9bolic.EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Accountool.Services;
public partial interface IKioskService
{
    Task<IEnumerable<Kiosk>> GetAll();
    Task<Kiosk?> GetById(int id);
    Task<int> Create(Kiosk entity);
    Task Update(Kiosk entity);
    Task Delete(int entityId);
    Task<IEnumerable<Kiosk>> GetForTown(int TownId);
    Task<IEnumerable<Kiosk>> GetForOrganization(int OrganizationId);
    Task<IEnumerable<Kiosk>> GetForKioskSection(int KioskSectionId);
}

public partial class KioskService : IKioskService
{
    private readonly IRepository<Kiosk> _repository;
    private readonly IDbContextScopeFactory _dbContextScopeFactory;
    public KioskService(IRepository<Kiosk> repository, IDbContextScopeFactory dbContextScopeFactory)
    {
        _repository = repository;
        _dbContextScopeFactory = dbContextScopeFactory;
    }

    public async Task<IEnumerable<Kiosk>> GetAll()
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().ToArrayAsync();
    }

    public async Task<Kiosk?> GetById(int id)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.GetById(id);
    }

    public async Task<int> Create(Kiosk entity)
    {
        using var scope = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted);
        _repository.Add(entity);
        await scope.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Update(Kiosk entity)
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

    public async Task<IEnumerable<Kiosk>> GetForTown(int TownId)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().Where(entity => entity.TownId == TownId).ToArrayAsync();
    }

    public async Task<IEnumerable<Kiosk>> GetForOrganization(int OrganizationId)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().Where(entity => entity.OrganizationId == OrganizationId).ToArrayAsync();
    }

    public async Task<IEnumerable<Kiosk>> GetForKioskSection(int KioskSectionId)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().Where(entity => entity.KioskSectionId == KioskSectionId).ToArrayAsync();
    }
}