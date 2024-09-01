namespace GoldShop.Application.Dtos;

public class PaymentRequest
{
    public string LoginAccount { get; set; }
    public long OrderId { get; set; }
    public long Amount { get; set; }
    public string CallBackUrl { get; set; }
    public string AdditionalData { get; set; }
    public string Originator { get; set; }
}