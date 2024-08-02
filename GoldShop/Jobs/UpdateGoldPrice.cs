using System.Text;
using System.Text.Json;
using GoldShop.Application.Jobs;
using GoldShop.Controllers;
using MediatR;
using Quartz;

namespace GoldShop.Jobs;

public class UpdateGoldPrice : IJob
{
    private readonly IMediator Mediator;

    public UpdateGoldPrice(IMediator mediator)
    {
        Mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        HttpClient client = new HttpClient();
        var jsonPayload = "{\"name\":\"value\"}";

        // Create an instance of StringContent
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // // Send a POST request to the specified URI
        HttpResponseMessage response =
            await client.PostAsync("https://webservice.tgnsrv.ir/Pr/Get/afshar7365/a09193647365a", content);

        // // Ensure the request was successful
        response.EnsureSuccessStatusCode();
        //
        // // Read the response content as a string
        string responseBody = await response.Content.ReadAsStringAsync();
        var currency = JsonSerializer.Deserialize<AdminController.CurrencyRates>(responseBody);
        await Mediator.Send(new UpdateGoldPriceCommand
        {
            GoldPrice = currency.YekGram18
        });
        
    }
}