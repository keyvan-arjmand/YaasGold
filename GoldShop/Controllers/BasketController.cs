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

public class BasketController : Controller
{
    private readonly IUnitOfWork _work;
    private readonly UserManager<User> _userManager;

    public BasketController(IUnitOfWork work, UserManager<User> userManager)
    {
        _work = work;
        _userManager = userManager;
    }

    public async Task<IActionResult> AddToBasket(int id, double weight, string size)
    {
        var basketlist = new List<BasketDto>();

        if (HttpContext.Session.GetString("basket") != null)
        {
            basketlist = JsonConvert.DeserializeObject<List<BasketDto>>(HttpContext.Session.GetString("basket"))
                .ToList();
        }

        basketlist.Add(new BasketDto
        {
            Size = size,
            Id = id,
            Weight = weight
        });
        HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basketlist));

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Basket()
    {
        ViewBag.PostMethod = await _work.GenericRepository<PostMethod>().TableNoTracking.ToListAsync();
        ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
        var basketProducts = new List<CheckOutDto>();
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

        ViewBag.BasketProd = basketProducts;
        ViewBag.Curency = await _work.GenericRepository<GoldPrice>().TableNoTracking.FirstOrDefaultAsync();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<BasketDto>>(HttpContext.Session.GetString("basket"))
                .ToList();
            ViewBag.BasketCount = basketList.Count;
        }
        else
        {
            ViewBag.BasketCount = 0;
        }


        return View();
    }

    public async Task<IActionResult> DiscountCheck(string code)
    {
        var result = await _work.GenericRepository<DiscountCode>().TableNoTracking
            .FirstOrDefaultAsync(x => x.Code == code && x.IsActive);
        if (result is { Count: > 0 })
        {
            HttpContext.Session.SetString("discountCode", JsonConvert.SerializeObject(result));
            return Ok();
        }
        else
        {
            throw new Exception();
        }
    }

    public async Task<IActionResult> CheckOut(int postMethod)
    {
        if (postMethod <= 0)
        {
            if (HttpContext.Session.GetString("postMethod") != null)
            {
                int postId = JsonConvert.DeserializeObject<int>(HttpContext.Session.GetString("postMethod"));
                ViewBag.PostMethodPy = await _work.GenericRepository<PostMethod>().TableNoTracking
                    .FirstOrDefaultAsync(x => x.Id == postId);
            }
        }
        else
        {
            ViewBag.PostMethodPy = await _work.GenericRepository<PostMethod>().TableNoTracking
                .FirstOrDefaultAsync(x => x.Id == postMethod);
            HttpContext.Session.SetString("postMethod", JsonConvert.SerializeObject(postMethod));
        }

        ViewBag.PostMethod = await _work.GenericRepository<PostMethod>().TableNoTracking.ToListAsync();
        ViewBag.State = await _work.GenericRepository<State>().TableNoTracking.ToListAsync();
        var discount = new DiscountCode();
        if (HttpContext.Session.GetString("discountCode") != null)
        {
            discount = JsonConvert.DeserializeObject<DiscountCode>(HttpContext.Session.GetString("discountCode"));
        }

        ViewBag.Discount = discount;
        ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
        var basketProducts = new List<CheckOutDto>();
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

        ViewBag.BasketProd = basketProducts;
        ViewBag.Curency = await _work.GenericRepository<GoldPrice>().TableNoTracking.FirstOrDefaultAsync();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<BasketDto>>(HttpContext.Session.GetString("basket"))
                .ToList();
            ViewBag.BasketCount = basketList.Count;
        }
        else
        {
            ViewBag.BasketCount = 0;
        }


        return View();
    }

    public IActionResult RemoveInBasket(int id)
    {
        var basketlist = new List<BasketDto>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            basketlist = JsonConvert.DeserializeObject<List<BasketDto>>(HttpContext.Session.GetString("basket"))
                .ToList();
        }

        basketlist.Remove(basketlist.First(x => x.Id == id));
        HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basketlist));
        return RedirectToAction("Index", "Home");
    }
    
    [HttpPost]
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

        HttpClient client = new HttpClient();
        //htttp yass bank and get Bank Url ***
        string requestUrl =
            $"https://bank.yaasgold.ir/Home/SalePayment?idFactor={factor.Id}&Amount={factor.Amount}";
        HttpResponseMessage response = await client.GetAsync(requestUrl);
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

}