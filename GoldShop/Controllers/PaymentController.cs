using System.ComponentModel.DataAnnotations;
using GoldShop.Application.Common.Utilities;
using GoldShop.Application.Dtos.Bank;
using GoldShop.Application.Interfaces;
using GoldShop.Comman;
using GoldShop.Domain.Entity.Factor;
using GoldShop.Domain.Entity.Product;
using GoldShop.Domain.Entity.User;
using GoldShop.Domain.Enums;
using GoldShop.Models;
using GoldShop.Views.Bank;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GoldShop.Controllers;

public class PaymentController : Controller
{
    private readonly IUnitOfWork _work;
    private readonly UserManager<User> _userManager;

    public PaymentController(IUnitOfWork work, UserManager<User> userManager)
    {
        _work = work;
        _userManager = userManager;
    }


    public class PaymentCallbackModel
    {
        public long? Token { get; set; }
        public int? Status { get; set; }

        [Display(Name = "شناسه سفارش")] public long? OrderId { get; set; }

        public int? TerminalNo { get; set; }

        [Display(Name = "شماره مرجع تراکنش")] public long? RRN { get; set; }

        [Display(Name = "کد وضعیت تراکنش")] public short? status { get; set; }

        [Display(Name = "شماره کارت هش")] public string HashCardNumber { get; set; }

        [Display(Name = "مبلغ")] public string Amount { get; set; }

        [Display(Name = "TSP Token")] public string TspToken { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateFactor(PaymentCallbackModel request)
    {
        var factor = await _work.GenericRepository<Factor>().TableNoTracking
            .FirstOrDefaultAsync(x => x.Id == request.OrderId);

        if (request.status == Helper.ParsianPaymentGateway.Successful && (request.Token ?? 0L) > 0L)
        {
            if (request.Status == Helper.ParsianPaymentGateway.Successful)
            {
                factor.HashCardNumber = request.HashCardNumber;
                factor.RRN = request.RRN;
                factor.StatusDescription = request.status.ToString();
                factor.TerminalNo = request.TerminalNo;
                factor.Token = request.Token.ToString();
                factor.BankStatus = BankStatus.Successful;
                factor.DescBank = "پرداخت موفق بانکی";
            }
        }

        if (request.Status == Helper.ParsianPaymentGateway.CancelPay)
        {
            factor.HashCardNumber = request.HashCardNumber;
            factor.RRN = request.RRN;
            factor.StatusDescription = request.status.ToString();
            factor.TerminalNo = request.TerminalNo;
            factor.Token = request.Token.ToString();
            factor.BankStatus = BankStatus.Cancel;
            factor.DescBank = "انصراف از پرداخت بانکی";
        }

        await _work.GenericRepository<Factor>().UpdateAsync(factor, CancellationToken.None);
        return RedirectToAction("CallBack", "Bank", request);
    }
}