namespace GoldShop.Application.Dtos;

public class PaymentResponse
{
    public short Status { get; set; }
    public string Message { get; set; }
    public long Token { get; set; }
}