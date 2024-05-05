using System.Data;
using Accountool.DataAccess;
using Accountool.Models;
using D9bolic.EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Accountool.Services;
public partial interface IClientProfileService
{
    Task<IEnumerable<ClientProfile>> GetAll();
    Task<ClientProfile?> GetById(int id);
    Task<int> Create(ClientProfile entity);
    Task Update(ClientProfile entity);
    Task Delete(int entityId);
    Task<IEnumerable<ClientProfile>> GetForUser(int UserId);
}

public partial class ClientProfileService : IClientProfileService
{
    private readonly IRepository<ClientProfile> _repository;
    private readonly IDbContextScopeFactory _dbContextScopeFactory;
    public ClientProfileService(IRepository<ClientProfile> repository, IDbContextScopeFactory dbContextScopeFactory)
    {
        _repository = repository;
        _dbContextScopeFactory = dbContextScopeFactory;
    }

    public async Task<IEnumerable<ClientProfile>> GetAll()
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().ToArrayAsync();
    }

    public async Task<ClientProfile?> GetById(int id)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.GetById(id);
    }

    public async Task<int> Create(ClientProfile entity)
    {
        using var scope = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted);
        _repository.Add(entity);
        await scope.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Update(ClientProfile entity)
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

    public async Task<IEnumerable<ClientProfile>> GetForUser(int UserId)
    {
        using var scope = _dbContextScopeFactory.CreateReadOnly();
        return await _repository.Get().Where(entity => entity.UserId == UserId).ToArrayAsync();
    }
}