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
        product.Name = request.Name;
        product.Brand = request.Brand;
        product.ImageBanner = request.ImageBanner;
        product.ImageThumb = request.ImageThumb;
        product.Image = request.Image;
        product.Weight = request.Weight;
        product.Wages = request.Wages;
        product.Inventory = request.Inventory;
        product.CategoryId = request.CategoryId;
        await _work.GenericRepository<Product>().UpdateAsync(product, cancellationToken);
    }
}