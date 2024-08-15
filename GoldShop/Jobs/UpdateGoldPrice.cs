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
                gold.PricePerGram = currency.YekGram18;
            }

            gold.PriceApi = currency.YekGram18;
            gold.UpdateTime = DateTime.Now;
            gold.YekGram18 = currency.YekGram18;
            gold.YekMesghal18 = currency.YekMesghal18;
            gold.SekehRob = currency.SekehRob;
            gold.SekehNim = currency.SekehNim;
            gold.SekehEmam = currency.SekehEmam;
            gold.SekehTamam = currency.SekehTamam;
            gold.SekehGerami = currency.SekehGerami;
            gold.OunceTala = currency.OunceTala;
            gold.YekMesghal17 = currency.YekMesghal17;
            gold.OunceNoghreh = currency.OunceNoghreh;
            gold.Pelatin = currency.Pelatin;
            gold.Dollar = currency.Dollar;
            gold.Euro = currency.Euro;
            gold.Derham = currency.Derham;
            gold.YekGram20 = currency.YekGram20;
            gold.YekGram21 = currency.YekGram21;
            await _work.GenericRepository<GoldPrice>().UpdateAsync(gold, CancellationToken.None);
        }
    }
}