using GoldShop.Domain.Common;

namespace GoldShop.Application.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<T> GenericRepository<T>() where T : BaseEntity;
}