using GoldShop.Application.Interfaces;
using GoldShop.Domain.Entity.Product;
using GoldShop.Domain.Entity.User;
using GoldShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GoldShop.Controllers;

public class BasketController : Controller
{
    private readonly IUnitOfWork _work;

    public BasketController(IUnitOfWork work)
    {
        _work = work;
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
}