using GoldShop.Application.Interfaces;
using GoldShop.Application.Jobs;
using GoldShop.Domain.Entity.Product;
using GoldShop.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GoldShop.Jobs;

public class UpdateGoldPriceCommandHandler:IRequestHandler<UpdateGoldPriceCommand>
{
    private readonly IUnitOfWork _work;

    public UpdateGoldPriceCommandHandler(IUnitOfWork work)
    {
        _work = work;
    }

    public async Task Handle(UpdateGoldPriceCommand request, CancellationToken cancellationToken)
    {
        var gold = await _work.GenericRepository<GoldPrice>().Table.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (gold.PriceType == PriceType.Auto)
        {
            gold.PricePerGram = request.GoldPrice;
        }
        gold.PriceApi = request.GoldPrice;
        gold.UpdateTime = DateTime.Now;
        await _work.GenericRepository<GoldPrice>().UpdateAsync(gold, cancellationToken);
    }
}