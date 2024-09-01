using System.ServiceModel;
using GoldShop.Application.Dtos;

namespace GoldShop.Application.Interfaces;

public interface IPaymentService
{
    [OperationContract]
    Task<PaymentResponse> SalePaymentAsync(PaymentRequest request);
}