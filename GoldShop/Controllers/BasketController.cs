using GoldShop.Application.Interfaces;
using GoldShop.Domain.Entity.Product;
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
        ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
        var basketProducts = new List<CheckOutDto>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            var basketList = JsonConvert.DeserializeObject<List<BasketDto>>(HttpContext.Session.GetString("basket")).ToList();
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

        return View();
    }
    public IActionResult RemoveInBasket(int id)
    {
        var basketlist = new List<BasketDto>();
        if (HttpContext.Session.GetString("basket") != null)
        {
            basketlist = JsonConvert.DeserializeObject<List<BasketDto>>(HttpContext.Session.GetString("basket")).ToList();
        }

        basketlist.Remove(basketlist.First(x=>x.Id==id));
        HttpContext.Session.SetString("basket", JsonConvert.SerializeObject(basketlist));
        return RedirectToAction("Index", "Home");
    }
}