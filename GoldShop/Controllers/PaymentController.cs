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
    private readonly HttpClient _httpClient;
    private readonly UserManager<User> _userManager;

    public PaymentController(IUnitOfWork work, HttpClient httpClient, UserManager<User> userManager)
    {
        _work = work;
        _httpClient = httpClient;
        _userManager = userManager;
    }

    public async Task<string> InsertFactor(FactorModel request)
    {
        var gold = await _work.GenericRepository<GoldPrice>().TableNoTracking.FirstOrDefaultAsync();
        var basketProducts = new List<CheckOutDto>();
        var postMethod = new PostMethod();
        var discountCode = new DiscountCode();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<BasketDto>>(HttpContext.Session.GetString("basket"))
                .ToList();
            foreach (var i in basketList)
            {
                var prod = await _work.GenericRepository<Product>().TableNoTracking.Include(x => x.Category)
                    .Include(x => x.GoldPrice).FirstOrDefaultAsync(x => x.Id == i.Id);
                basketProducts.Add(new CheckOutDto
                {
                    Size = i.Size,
                    GoldPrice = prod.GoldPrice,
                    Brand = prod.Brand,
                    Desc = prod.Desc,
                    Image = prod.Image,
                    Id = prod.Id,
                    WagesAmount = prod.WagesAmount,
                    Weight = i.Weight,
                    Name = prod.Name,
                    WagesPercentage = prod.WagesPercentage
                });
            }
        }

        if (HttpContext.Session.GetString("postMethod") != null)
        {
            int postId = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("postMethod"));
            postMethod = await _work.GenericRepository<PostMethod>().TableNoTracking
                .FirstOrDefaultAsync(x => x.Id == postId);
        }

        if (HttpContext.Session.GetString("discountCode") != null)
        {
            discountCode = JsonConvert.DeserializeObject<DiscountCode>(HttpContext.Session.GetString("discountCode"));
        }

        var prods = basketProducts.Sum(x =>
            x.GoldPrice.PricePerGram.GetPrice(x.Weight, x.WagesAmount, x.WagesPercentage)) - discountCode.Amount;
        if (postMethod.Price > 0)
        {
            prods += postMethod.Price;
        }

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
        var address = new UserAddress
        {
            Address = request.Address,
            Name = request.Name,
            UserId = user.Id,
            Number = request.Number,
            PostCode = request.PostCode,
            StateId = request.StateId
        };
        await _work.GenericRepository<UserAddress>().AddAsync(address, CancellationToken.None);
        var factor = new Factor
        {
            StatusEdit = Status.Pending,
            GoldRate = gold.YekGram18,
            Amount = prods,
            Desc = request.Desc,
            FactorCode = Helpers.CodeGenerator(user.Id, DateTime.Now.Month.ToString()),
            UserId = user.Id,
            DiscountAmount = discountCode.Amount,
            DiscountCode = discountCode.Code,
            InsertDate = DateTime.Now,
            PostMethodId = postMethod.Id,
            UserAddressId = address.Id,
            DescBank = "در انتظار پرداخت",
            BankStatus = BankStatus.Pending,
        };
        await _work.GenericRepository<Factor>().AddAsync(factor, CancellationToken.None);

        foreach (var i in basketProducts)
        {
            await _work.GenericRepository<FactorProduct>().AddAsync(new FactorProduct
            {
                Count = 1,
                Size = i.Size,
                Weight = i.Weight,
                FactorId = factor.Id,
                ProductId = i.Id,
            }, CancellationToken.None);
        }

        //htttp yass bank and get Bank Url ***
        string requestUrl =
            $"https://bank.yaasgold.ir/Home/SalePayment?idFactor={factor.Id}&Amount={factor.Amount}";
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