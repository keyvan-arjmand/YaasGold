using GoldShop.Application.Common.Exceptions;
using GoldShop.Application.Interfaces;
using GoldShop.Domain.Entity.Product;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace GoldShop.Application.Admin.V1.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
{
    private readonly IUnitOfWork _work;

    public CreateProductCommandHandler(IUnitOfWork work)
    {
        _work = work;
    }

    public async Task Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var gold = await _work.GenericRepository<GoldPrice>().TableNoTracking
            .FirstOrDefaultAsync(x => x.Id == 1, cancellationToken: cancellationToken);
        if (gold == null) throw new NotFoundException("gold price not found");
        var cat = await _work.GenericRepository<Category>().TableNoTracking
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken: cancellationToken);
        if (cat == null) throw new NotFoundException("cat not found");
        await _work.GenericRepository<Product>().AddAsync(new Product
        {
            Weight = request.Weight,
            Image = request.Image,
            ImageThumb = request.ImageThumb,
            Brand = request.Brand,
            Name = request.Name,
            Inventory = request.Inventory,
            Wages = request.Wages,
            ImageBanner = request.ImageBanner,
            InsertDate = DateTime.Now,
            IsDelete = false,
            GoldPriceId = gold.Id,
            CategoryId = cat.Id
        }, cancellationToken);
    }
}