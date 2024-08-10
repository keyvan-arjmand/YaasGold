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
        var weight = request.Weight.Split(",").Select(Convert.ToDouble).ToList();
        var prod = new Product
        {
            Image = request.Image,
            ImageThumb = request.ImageThumb,
            Brand = request.Brand,
            Name = request.Name,
            Inventory = request.Inventory,
            WagesAmount = request.Wages,
            ImageBanner = request.ImageBanner,
            InsertDate = DateTime.Now,
            IsDelete = false,
            GoldPriceId = gold.Id,
            CategoryId = cat.Id,
            Size = request.Size,
            Desc = request.Desc,
            WagesPercentage = request.WagesPercentage,
        };
        for (int i = 1; i <= weight.Count; i++)
        {
            if (i == 1)
            {
                prod.Weight = weight[0];
            }
            if (i == 2)
            {
                prod.Weight2 = weight[1];
            }
            if (i == 3)
            {
                prod.Weight3 = weight[2];
            }
            if (i == 4)
            {
                prod.Weight4 = weight[3];
            }
            if (i == 5)
            {
                prod.Weight5 = weight[4];
            }
        }
        await _work.GenericRepository<Product>().AddAsync(prod, cancellationToken);
    }
}