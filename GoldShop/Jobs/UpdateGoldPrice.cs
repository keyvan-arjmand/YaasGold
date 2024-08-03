using System.Text;
using System.Text.Json;
using GoldShop.Application.Interfaces;
using GoldShop.Application.Jobs;
using GoldShop.Controllers;
using GoldShop.Domain.Entity.Product;
using GoldShop.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace GoldShop.Jobs;

public class UpdateGoldPrice : IJob
{
    private readonly IUnitOfWork _work;

    public UpdateGoldPrice(IUnitOfWork work)
    {
        _work = work;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response =
            await client.GetAsync("https://webservice.tgnsrv.ir/Pr/Get/afshar7365/a09193647365a");
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var currency = JsonSerializer.Deserialize<AdminController.CurrencyRates>(responseBody);
        if (currency.YekGram18 > 0)
        {
            var gold = await _work.GenericRepository<GoldPrice>().Table.FirstOrDefaultAsync();
            if (gold.PriceType == PriceType.Auto)
            {
                gold.PricePerGram = currency.YekGram18 ;
            }

            gold.PriceApi = currency.YekGram18 ;
            gold.UpdateTime = DateTime.Now;
            await _work.GenericRepository<GoldPrice>().UpdateAsync(gold, CancellationToken.None);
        }
    }
}