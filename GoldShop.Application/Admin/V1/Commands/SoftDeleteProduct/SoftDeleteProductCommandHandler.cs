using GoldShop.Application.Common.Exceptions;
using GoldShop.Application.Interfaces;
using GoldShop.Domain.Entity.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoldShop.Application.Admin.V1.Commands.SoftDeleteProduct;

public class SoftDeleteProductCommandHandler : IRequestHandler<SoftDeleteProductCommand>
{
    private readonly IUnitOfWork _work;

    public SoftDeleteProductCommandHandler(IUnitOfWork work)
    {
        _work = work;
    }

    public async Task Handle(SoftDeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _work.GenericRepository<Product>().Table
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product == null) throw new NotFoundException("product not found");
        product.IsDelete = true;
        await _work.GenericRepository<Product>().UpdateAsync(product, cancellationToken);
    }
}