using GoldShop.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoldShop.Controllers;

public class BankController : Controller
{
    private readonly IPaymentService _paymentService;

    public BankController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public ActionResult CallBack()
    {
        return View();
    }


}