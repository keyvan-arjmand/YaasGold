using MediatR;

namespace GoldShop.Application.Jobs;

public class UpdateGoldPriceCommand : IRequest
{
    public double GoldPrice { get; set; }
}