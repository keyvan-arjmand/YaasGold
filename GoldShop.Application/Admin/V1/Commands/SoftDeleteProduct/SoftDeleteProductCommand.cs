using MediatR;

namespace GoldShop.Application.Admin.V1.Commands.SoftDeleteProduct;

public class SoftDeleteProductCommand:IRequest
{
    public long Id { get; set; }
}