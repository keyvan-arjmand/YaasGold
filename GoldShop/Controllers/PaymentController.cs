using System.ComponentModel.DataAnnotations;
using GoldShop.Application.Common.Utilities;
using GoldShop.Application.Dtos.Bank;
using GoldShop.Application.Interfaces;
using GoldShop.Comman;
using GoldShop.Domain.Entity.Factor;
using GoldShop.Domain.Enums;
using GoldShop.Views.Bank;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoldShop.Controllers;

public class PaymentController : Controller
{
    private readonly IUnitOfWork _work;
    private readonly HttpClient _httpClient;

    public PaymentController(IUnitOfWork work, HttpClient httpClient)
    {
        _work = work;
        _httpClient = httpClient;
    }

    public async Task<string> InsertFactor(FactorModel request)
    {
        var factor = new Factor
        {
            Statuss = Status.Pending,
            GoldRate = request.GoldRate,
            Amount = request.Amount,
            Desc = request.Desc,
            FactorCode = Helpers.CodeGenerator(request.UserId, DateTime.Now.Month.ToString()),
            UserId = request.UserId,
            DiscountAmount = request.DiscountAmount,
            DiscountCode = request.DiscountCode,
            InsertDate = DateTime.Now,
            PostMethodId = request.PostMethodId,
            UserAddressId = request.UserAddressId,
            DescBank = "در انتظار پرداخت",
            BankStatus = BankStatus.Pending,
        };
        await _work.GenericRepository<Factor>().AddAsync(factor, CancellationToken.None);

        foreach (var i in request.Products)
        {
            await _work.GenericRepository<FactorProduct>().AddAsync(new FactorProduct
            {
                Count = 1,
                Size = i.Size,
                Weight = i.Weight,
                FactorId = factor.Id,
                ProductId = i.ProductId,
            }, CancellationToken.None);
        }

        //htttp yass bank and get Bank Url ***
        string requestUrl = $"https://yaasgold.ir/Home/SalePayment?idFactor={factor.Id}&Amount={factor.Amount}";
        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
        if (response.IsSuccessStatusCode)
        {
            // Read the response content as a string
            string jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a BankTokenStatus object
            BankTokenStatus? bankTokenStatus =
                Newtonsoft.Json.JsonConvert.DeserializeObject<BankTokenStatus>(jsonResponse);
            return bankTokenStatus.Token;
        }

        return string.Empty;
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
    public async Task<IActionResult> UpdateFactor(string HashCardNumber,long RRN,short status,int TerminalNo ,BankStatus BankStatus)
    {
        var factor = await _work.GenericRepository<Factor>().TableNoTracking
            .FirstOrDefaultAsync(x => x.Id == request.OrderId);

        if (request.status == Helper.ParsianPaymentGateway.Successful && (request.Token ?? 0L) > 0L)
        {
            //درصورتی که موفق باشد، باید خدمات یا کالا به کاربر پرداخت کننده ارائه شود
            if (request.Status == Helper.ParsianPaymentGateway.Successful)
            {
                factor.HashCardNumber = request.HashCardNumber;
                factor.RRN = request.RRN;
                factor.status = request.status.ToString();
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
            factor.status = request.status.ToString();
            factor.TerminalNo = request.TerminalNo;
            factor.Token = request.Token.ToString();
            factor.BankStatus = BankStatus.Cancel;
            factor.DescBank = "انصراف از پرداخت بانکی";
        }

        await _work.GenericRepository<Factor>().UpdateAsync(factor, CancellationToken.None);
        return View();
    }
}