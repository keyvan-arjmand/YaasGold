using System.Diagnostics;
using GoldShop.Application.Common.Exceptions;
using GoldShop.Application.Interfaces;
using GoldShop.Comman;
using GoldShop.Domain.Entity.Product;
using GoldShop.Domain.Entity.User;
using Microsoft.AspNetCore.Mvc;
using GoldShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;

namespace GoldShop.Controllers;

public class HomeController : Controller
{
    private readonly IUnitOfWork _work;
    private readonly ILogger<HomeController> _logger;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;


    public HomeController(ILogger<HomeController> logger, IUnitOfWork work, RoleManager<Role> roleManager,
        UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _logger = logger;
        _work = work;
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<ActionResult> Index()
    {
        ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
        ViewBag.PopProducts = await _work.GenericRepository<Product>()
            .TableNoTracking
            .Include(x => x.Category)
            .Include(x => x.GoldPrice)
            .Take(15).ToListAsync();
        ViewBag.SpecialProducts = await _work.GenericRepository<Product>()
            .TableNoTracking
            .Include(x => x.Category)
            .Include(x => x.GoldPrice)
            .Take(15).ToListAsync();
        ViewBag.NewProducts = await _work.GenericRepository<Product>()
            .TableNoTracking
            .Include(x => x.Category)
            .Include(x => x.GoldPrice)
            .OrderBy(x => x.InsertDate)
            .Take(15).ToListAsync();
        return View();
    }

    public async Task<ActionResult> ContactUs()
    {
        ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
        return View();
    }

    public async Task<ActionResult> Category(int id, int page = 1)
    {
        ViewBag.searchHistory = new SearchHistory()
            { Search = string.Empty, Id = id, MinPrice = 0, MaxPrice = 0, Page = page };
        ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
        ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.Category)
            .Include(x => x.GoldPrice)
            .Where(x => x.CategoryId == id)
            .Skip((page - 1) * 10).Take(10)
            .ToListAsync();
        return View();
    }

    public async Task<ActionResult> ProductDetail(long id)
    {
        ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
        var prod = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.Category)
            .Include(x => x.GoldPrice)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (prod == null) throw new NotFoundException("product not found");
        ViewBag.product = prod;
        ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
            .Include(x => x.Category)
            .Include(x => x.GoldPrice)
            .Take(10)
            .ToListAsync();
        return View();
    }

    public async Task<ActionResult> Search(string search, int id, int page = 1)
    {
        ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
        ViewBag.searchHistory = new SearchHistory()
            { Search = search, Id = id, MinPrice = 0, MaxPrice = 0, Page = page };
        switch (string.IsNullOrWhiteSpace(search), id <= 0)
        {
            case (true, true):
                ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                    .Include(x => x.Category)
                    .Include(x => x.GoldPrice)
                    .Skip((page - 1) * 10).Take(10)
                    .ToListAsync();
                break;
            case (true, false):
                ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                    .Include(x => x.Category)
                    .Include(x => x.GoldPrice)
                    .Where(x => x.CategoryId == id)
                    .Skip((page - 1) * 10).Take(10)
                    .ToListAsync();
                break;
            case (false, true):
                ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                    .Include(x => x.Category)
                    .Include(x => x.GoldPrice)
                    .Where(x => x.Name.Contains(search) || x.Brand.Contains(search))
                    .Skip((page - 1) * 10).Take(10)
                    .ToListAsync();
                break;
            case (false, false):
                ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                    .Include(x => x.Category)
                    .Include(x => x.GoldPrice)
                    .Where(x => x.Name.Contains(search) || x.Brand.Contains(search) && x.CategoryId == id)
                    .Skip((page - 1) * 10).Take(10)
                    .ToListAsync();
                break;
        }

        return View("Category");
    }

    public async Task<ActionResult> FilterByPrice(string search, int id, double minPrice, double maxPrice, int page = 1)
    {
        ViewBag.searchHistory = new SearchHistory()
            { Search = search, Id = id, MinPrice = minPrice, MaxPrice = maxPrice, Page = page };
        ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
        var isFilter = minPrice >= 0 && maxPrice > 0 ? true : false;
        switch (string.IsNullOrWhiteSpace(search), id <= 0)
        {
            case (true, true):
                if (isFilter)
                {
                    ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                        .Include(x => x.Category)
                        .Include(x => x.GoldPrice)
                        .Where(x => minPrice <= (x.Weight * x.GoldPrice.PricePerGram) +
                                    (x.Weight * x.Wages) +
                                    (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100) +
                                    (((x.Weight * x.Wages) +
                                      (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100)) * 9 /
                                     100) &&
                                    maxPrice >= (x.Weight * x.GoldPrice.PricePerGram) +
                                    (x.Weight * x.Wages) +
                                    (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100) +
                                    (((x.Weight * x.Wages) +
                                      (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100)) * 9 /
                                     100))
                        .Skip((page - 1) * 10).Take(10)
                        .ToListAsync();
                }
                else
                {
                    ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                        .Include(x => x.Category)
                        .Include(x => x.GoldPrice)
                        .Skip((page - 1) * 10).Take(10)
                        .ToListAsync();
                }

                break;
            case (true, false):
                if (isFilter)
                {
                    ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                        .Include(x => x.Category)
                        .Include(x => x.GoldPrice)
                        .Where(x => x.CategoryId == id &&
                                    minPrice <= (x.Weight * x.GoldPrice.PricePerGram) +
                                    (x.Weight * x.Wages) +
                                    (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100) +
                                    (((x.Weight * x.Wages) +
                                      (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100)) * 9 /
                                     100) &&
                                    maxPrice >= (x.Weight * x.GoldPrice.PricePerGram) +
                                    (x.Weight * x.Wages) +
                                    (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100) +
                                    (((x.Weight * x.Wages) +
                                      (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100)) * 9 /
                                     100))
                        .Skip((page - 1) * 10).Take(10)
                        .ToListAsync();
                }
                else
                {
                    ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                        .Include(x => x.Category)
                        .Include(x => x.GoldPrice)
                        .Where(x => x.CategoryId == id)
                        .Skip((page - 1) * 10).Take(10)
                        .ToListAsync();
                }

                break;
            case (false, true):
                if (isFilter)
                {
                    ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                        .Include(x => x.Category)
                        .Include(x => x.GoldPrice)
                        .Where(x => x.Name.Contains(search) || x.Brand.Contains(search) &&
                            minPrice <= (x.Weight * x.GoldPrice.PricePerGram) +
                            (x.Weight * x.Wages) +
                            (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100) +
                            (((x.Weight * x.Wages) +
                              (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100)) * 9 / 100) &&
                            maxPrice >= (x.Weight * x.GoldPrice.PricePerGram) +
                            (x.Weight * x.Wages) +
                            (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100) +
                            (((x.Weight * x.Wages) +
                              (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100)) * 9 / 100))
                        .Skip((page - 1) * 10).Take(10)
                        .ToListAsync();
                }
                else
                {
                    ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                        .Include(x => x.Category)
                        .Include(x => x.GoldPrice)
                        .Where(x => x.Name.Contains(search) || x.Brand.Contains(search))
                        .Skip((page - 1) * 10).Take(10)
                        .ToListAsync();
                }

                break;
            case (false, false):
                if (isFilter)
                {
                    ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                        .Include(x => x.Category)
                        .Include(x => x.GoldPrice)
                        .Where(x => x.Name.Contains(search) || x.Brand.Contains(search) && x.CategoryId == id &&
                            minPrice <=
                            (x.Weight * x.GoldPrice.PricePerGram) +
                            (x.Weight * x.Wages) +
                            (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100) +
                            (((x.Weight * x.Wages) +
                              (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100)) * 9 / 100) &&
                            maxPrice >= (x.Weight * x.GoldPrice.PricePerGram) +
                            (x.Weight * x.Wages) +
                            (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100) +
                            (((x.Weight * x.Wages) +
                              (((x.Weight * x.Wages) + (x.Weight * x.GoldPrice.PricePerGram)) * 7 / 100)) * 9 / 100))
                        .Skip((page - 1) * 10).Take(10)
                        .ToListAsync();
                }
                else
                {
                    ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                        .Include(x => x.Category)
                        .Include(x => x.GoldPrice)
                        .Where(x => x.Name.Contains(search) || x.Brand.Contains(search) && x.CategoryId == id)
                        .Skip((page - 1) * 10).Take(10)
                        .ToListAsync();
                }

                break;
        }

        return View("Category");
    }

    public async Task<ActionResult> AboutUs()
    {
        ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
        return View();
    }

    public async Task<ActionResult> Login()
    {
        ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<ActionResult> Error()
    {
        ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public double GetPrice(double goldPrice, double weight, double wages)
    {
        var gold = weight * goldPrice;
        var wagesGold = wages * weight;
        var profit = (gold + wagesGold) * 7 / 100;
        var tax = (wagesGold + profit) * 9 / 100;
        return gold + wagesGold + tax + profit;
    }

    public async Task InsertUser(string number, string password, string email)
    {
        if (!await _userManager.Users.AnyAsync(x => x.PhoneNumber == number))
        {
            var user = new Domain.Entity.User.User
            {
                Family = "",
                Name = "user",
                PhoneNumber = number,
                Email = email,
                Password = password,
                InsertDate = DateTime.Now,
                UserName = number,
                SecurityStamp = string.Empty,
                CityId = 1,
            };
            if (!await _roleManager.RoleExistsAsync("user"))
            {
                await _roleManager.CreateAsync(new Role
                {
                    Name = "user"
                });
            }

            await _userManager.CreateAsync(user, password);
            await _userManager.AddToRoleAsync(user, "user");
            await _userManager.UpdateAsync(user);
            RedirectToAction("Index");
        }
        else
        {
            RedirectToAction("Index");
        }
       
    }

    public async Task UserLogin(string number, string password)
    {
        var user = await _userManager.FindByNameAsync(number);
        if (user != null)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _signInManager.PasswordSignInAsync(user, password, true, false);
            RedirectToAction("Profile");
        }
        else
        {
            RedirectToAction("Index");
        }
    }

    public async Task<ActionResult> Profile()
    {
        ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();

        return View();
    }
}