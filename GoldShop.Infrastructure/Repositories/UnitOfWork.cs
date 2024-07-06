using System.Collections;
using GoldShop.Application.Interfaces;
using GoldShop.Domain;
using GoldShop.Domain.Common;

namespace GoldShop.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly Db _db;
    private Hashtable? _repositories;

    public UnitOfWork(Db db)
    {
        _db = db;
    }

    public IGenericRepository<T> GenericRepository<T>() where T : BaseEntity
    {
        if (_repositories is null)
            _repositories = new Hashtable();

        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>);
            var repositoryInstance =
                Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _db);

            _repositories.Add(type, repositoryInstance);
        }

        return (IGenericRepository<T>)_repositories[type];
    }
}