using System.ServiceModel;
using GoldShop.Application.Dtos;
using GoldShop.Application.Interfaces;

namespace GoldShop.Infrastructure.Repositories;

public class PaymentService:IPaymentService
{
    private readonly string _serviceUrl = "https://pec.shaparak.ir/NewIPGServices/Sale/SaleService.asmx";

    public async Task<PaymentResponse> SalePaymentAsync(PaymentRequest request)
    {
        var client = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        var endpoint = new EndpointAddress(_serviceUrl);
        var channelFactory = new ChannelFactory<IPaymentService>(client, endpoint);
        var service = channelFactory.CreateChannel();

        var response = await service.SalePaymentAsync(request);

        return response;
    }
}