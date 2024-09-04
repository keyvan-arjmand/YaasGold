using GoldShop.Application.Common.Exceptions;
using GoldShop.Application.Interfaces;
using GoldShop.Domain.Entity.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoldShop.Application.Admin.V1.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IUnitOfWork _work;

    public UpdateProductCommandHandler(IUnitOfWork work)
    {
        _work = work;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var cat = await _work.GenericRepository<Category>().TableNoTracking
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken: cancellationToken);
        if (cat == null) throw new NotFoundException("cat not found");
        var product = await _work.GenericRepository<Product>().Table
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product == null) throw new NotFoundException("product not found");
        var weight = request.Weight.Split(",").Select(Convert.ToDouble).ToList();
        product.Name = request.Name;
        product.Brand = request.Brand;
        product.ImageBanner = string.IsNullOrEmpty(request.ImageBanner)
            ? product.ImageBanner
            : request.ImageBanner;
        product.ImageThumb = string.IsNullOrEmpty(request.ImageThumb)
            ? product.ImageThumb
            : request.ImageThumb;
        product.Image = string.IsNullOrEmpty(request.Image)
            ? product.Image
            : request.Image;
        product.IsSpecial = request.IsSpec;
        for (int i = 1; i <= weight.Count; i++)
        {
            if (i == 1)
            {
                product.Weight = weight[0];
            }

            if (i == 2)
            {
                product.Weight2 = weight[1];
            }

            if (i == 3)
            {
                product.Weight3 = weight[2];
            }

            if (i == 4)
            {
                product.Weight4 = weight[3];
            }

            if (i == 5)
            {
                product.Weight5 = weight[4];
            }
        }

        product.WagesAmount = request.Wages;
        product.Inventory = request.Inventory;
        product.CategoryId = request.CategoryId;
        product.Size = request.Size;
        product.WagesPercentage = request.WagesPercentage;
        await _work.GenericRepository<Product>().UpdateAsync(product, cancellationToken);
    }
}