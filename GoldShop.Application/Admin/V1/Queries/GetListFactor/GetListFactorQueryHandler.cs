using GoldShop.Application.Dtos.Factor;
using GoldShop.Application.Dtos.User;
using GoldShop.Application.Interfaces;
using GoldShop.Domain.Entity.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoldShop.Application.Admin.V1.Queries.GetListFactor;

public class GetListFactorQueryHandler : IRequestHandler<GetListFactorQuery, List<FactorDto>>
{
    private readonly IUnitOfWork _work;

    public GetListFactorQueryHandler(IUnitOfWork work)
    {
        _work = work;
    }

    public async Task<List<FactorDto>> Handle(GetListFactorQuery request, CancellationToken cancellationToken)
    {
        return await _work.GenericRepository<Factor>().TableNoTracking
            .Include(x => x.Products).ThenInclude(x => x.Product).ThenInclude(x => x.Category)
            .OrderBy(x => x.DateTime)
            .Select(x => new FactorDto
            {
                Id = x.Id,
                Products = x.Products.Select(y => new ProductFactorDto
                {
                    Id = y.ProductId,
                    Inventory = y.Product.Inventory,
                    Brand = y.Product.Brand,
                    Name = y.Product.Name,
                    Wages = y.Product.Wages,
                    Weight = y.Product.Weight,
                    CategoryName = y.Product.Category.Name
                }).ToList(),
                DateTime = x.DateTime,
                Address = x.Address,
            }).Skip((request.Page - 1) * 10).Take(10).ToListAsync(cancellationToken: cancellationToken);
    }
}