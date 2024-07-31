using GoldShop.Application.Admin.V1.Commands.AdminExist;
using GoldShop.Application.Admin.V1.Commands.ConfirmCodAdmin;
using GoldShop.Application.Admin.V1.Commands.ConfirmPasswordAdmin;
using GoldShop.Application.Admin.V1.Commands.CreateProduct;
using GoldShop.Application.Admin.V1.Commands.LoginCodAdmin;
using GoldShop.Application.Admin.V1.Commands.SoftDeleteProduct;
using GoldShop.Application.Admin.V1.Commands.UpdateProduct;
using GoldShop.Application.Admin.V1.Queries.CheckAdminNumber;
using GoldShop.Application.Admin.V1.Queries.GetListFactor;
using GoldShop.Application.Admin.V1.Queries.GetListUser;
using GoldShop.Application.Dtos;
using GoldShop.Application.Dtos.Factor;
using GoldShop.Application.Dtos.Products;
using GoldShop.Application.Dtos.User;
using GoldShop.Application.Interfaces;
using GoldShop.Domain.Entity.Page;
using GoldShop.Domain.Entity.Product;
using GoldShop.Domain.Entity.User;
using GoldShop.Domain.Enums;
using GoldShop.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoldShop.Controllers;

public class AdminController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IUnitOfWork _work;

    public AdminController(UserManager<User> userManager, SignInManager<User> signInManager, IMediator mediator,
        IWebHostEnvironment webHostEnvironment, RoleManager<Role> roleManager, IUnitOfWork work)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mediator = mediator;
        _webHostEnvironment = webHostEnvironment;
        _roleManager = roleManager;
        _work = work;
    }

    public async Task<ActionResult> Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            ViewBag.orderCount =
                _work.GenericRepository<Factor>().TableNoTracking.Count(x => x.Status == Status.Pending);
            ViewBag.orderCountToday = _work.GenericRepository<Factor>().TableNoTracking
                .Count(x => x.DateTime == DateTime.Today);
            ViewBag.newUsers = _userManager.Users.Count(x => x.InsertDate >= DateTime.Today.AddDays(-15));
            ViewBag.allUsers = _userManager.Users.Count();
            ViewBag.OrderStatus = new Init2Obj
            {
                Fit = _work.GenericRepository<Factor>().TableNoTracking.Count(x => x.Status == Status.Accepted),
                All = _work.GenericRepository<Factor>().TableNoTracking.Count()
            };
            ViewBag.ProductInventory = new Init2Obj
            {
                Fit = _work.GenericRepository<Product>().TableNoTracking.Count(x => x.Inventory > 0),
                All = _work.GenericRepository<Product>().TableNoTracking.Count()
            };
            ViewBag.allUsersView = await _userManager.Users.Take(6).Select(x => new UserDto
            {
                Id = x.Id,
                InsertDate = x.InsertDate,
                Name = x.Name,
                Family = x.Family,
                ImageName = x.ImageName
            }).ToListAsync();
            ViewBag.NewProducts =
                await _work.GenericRepository<Product>().TableNoTracking
                    .Include(x => x.Category)
                    .OrderBy(x => x.InsertDate).Take(4).Select(x =>
                        new ProductFactorDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Brand = x.Brand,
                            Inventory = x.Inventory,
                            Wages = x.Wages,
                            Weight = x.Weight,
                            Image = x.Image,
                            ImageBanner = x.ImageBanner,
                            ImageThumb = x.ImageThumb,
                            CategoryName = x.Category.Name
                        }).ToListAsync();

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> LoginPassword(string phoneNumber)
    {
        ViewBag.exUser = await _mediator.Send(new AdminExistCommand(phoneNumber));
        return View("LoginPassword");
    }

    public async Task<ActionResult> GoToLoginWithPassword()
    {
        return View("LoginPassword");
    }

    public async Task<ActionResult> GoToLoginWithCode()
    {
        return View("ConfirmCode");
    }

    public async Task<ActionResult> CreateProduct(CreateProductDto request)
    {
        var up = new Upload(_webHostEnvironment);
        CreateProductCommand req = new CreateProductCommand
        {
            Brand = request.Brand,
            Inventory = request.Inventory,
            Wages = request.Wages,
            Weight = request.Weight,
            Name = request.Name,
            CategoryId = request.CategoryId,
        };
        if (request.Image != null)
        {
            req.Image = up.Uploadfile(request.Image, "Product");
        }

        if (request.ImageBanner != null)
        {
            req.ImageBanner = up.Uploadfile(request.ImageBanner, "Product");
        }

        if (request.ImageThumb != null)
        {
            req.ImageThumb = up.Uploadfile(request.ImageThumb, "Product");
        }

        await _mediator.Send(req);
        return RedirectToAction("Product");
    }

    public async Task<ActionResult> UpdateProduct(UpdateProductDto request)
    {
        var up = new Upload(_webHostEnvironment);
        UpdateProductCommand req = new UpdateProductCommand
        {
            Brand = request.Brand,
            Inventory = request.Inventory,
            Wages = request.Wages,
            Weight = request.Weight,
            Name = request.Name,
            CategoryId = request.CategoryId,
            Id = request.Id,
            Image = request.Image != null
                ? up.Uploadfile(request.Image, "Product")
                : request.ImageName,
            ImageThumb = request.ImageThumb != null
                ? up.Uploadfile(request.ImageThumb, "Product")
                : request.ImageThumbName,
            ImageBanner = request.ImageBanner != null
                ? up.Uploadfile(request.ImageBanner, "Product")
                : request.ImageBannerName,
        };
        await _mediator.Send(req);
        return RedirectToAction("Product");
    }

    public async Task initAdmin()
    {
        var user = new Domain.Entity.User.User
        {
            Family = "arj",
            Name = "keyvan",
            PhoneNumber = "09211129482",
            Email = "keyvan.arjmnd@gmail.com",
            Password = "1111",
            InsertDate = DateTime.Now,
            UserName = "09211129482",
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

        if (!await _roleManager.RoleExistsAsync("admin"))
        {
            await _roleManager.CreateAsync(new Role
            {
                Name = "admin"
            });
        }

        await _userManager.CreateAsync(user, "1111");
        await _userManager.AddToRoleAsync(user, "user");
        await _userManager.AddToRoleAsync(user, "admin");
        await _userManager.UpdateAsync(user);
    }

    public async Task<ActionResult> LoginCod(string phoneNumber)
    {
        ViewBag.exUser = await _mediator.Send(new LoginCodAdminCommand(phoneNumber));
        return View("ConfirmCode");
    }

    public async Task<ActionResult> ConfirmPassword(string password, string phoneNumber)
    {
        await _mediator.Send(new ConfirmPasswordAdminCommand(password, phoneNumber));
        return Ok();
    }

    public async Task<ActionResult> CheckPhoneNumber(string phoneNumber)
    {
        await _mediator.Send(new CheckAdminNumberQuery
        {
            PhoneNumber = phoneNumber
        });
        return View("Login");
    }

    public async Task<ActionResult> ConfirmCode(string code, string phoneNumber)
    {
        await _mediator.Send(new ConfirmCodAdminCommand(phoneNumber, code));
        return Ok();
    }

    public async Task<ActionResult> Factor(int page = 1)
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.factorsPage = page;
            ViewBag.facTors = await _mediator.Send(new GetListFactorQuery(page));
            return View("Factor");
        }
        else
        {
            return View("Index");
        }
    }

    public async Task<ActionResult> UserList(int page = 1)
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.users = await _mediator.Send(new GetListUserQuery(page));
            ViewBag.productsPage = page;
            return View("User");
        }
        else
        {
            return View("Index");
        }
    }

    public async Task<ActionResult> SetManualGoldPrice(double price)
    {
        var gold = await _work.GenericRepository<GoldPrice>().TableNoTracking.FirstAsync(x => x.Id == 1);
        gold.PricePerGram = price;
        gold.PriceType = PriceType.Manual;
        await _work.GenericRepository<GoldPrice>().UpdateAsync(gold, new CancellationToken());
        return RedirectToAction("Product");
    }

    public async Task<ActionResult> SoftDeleteProduct(int id)
    {
        await _mediator.Send(new SoftDeleteProductCommand
        {
            Id = id
        });
        return RedirectToAction("Product");
    }

    public async Task<ActionResult> Product(string? search, int catId, int page = 1)
    {
        if (User.Identity.IsAuthenticated)
        {
            ViewBag.productCount = await _work.GenericRepository<Product>().TableNoTracking.CountAsync();
            ViewBag.inventory =
                await _work.GenericRepository<Product>().TableNoTracking.CountAsync(x => x.Inventory > 0);
            ViewBag.orders = await _work.GenericRepository<Factor>().TableNoTracking.CountAsync();
            ViewBag.categories = await _work.GenericRepository<Category>().TableNoTracking.ToListAsync();
            ViewBag.goldPrice = await _work.GenericRepository<GoldPrice>().TableNoTracking.FirstAsync();
            switch (string.IsNullOrWhiteSpace(search ?? string.Empty), catId <= 0)
            {
                case (true, true):
                    ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                        .Include(x => x.Category)
                        .Skip((page - 1) * 10).Take(10)
                        .ToListAsync();
                    break;
                case (true, false):
                    ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                        .Include(x => x.Category)
                        .Where(x => x.CategoryId == catId)
                        .Skip((page - 1) * 10).Take(10)
                        .ToListAsync();
                    break;
                case (false, true):
                    ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                        .Include(x => x.Category)
                        .Where(x => x.Name.Contains(search) || x.Brand.Contains(search))
                        .Skip((page - 1) * 10).Take(10)
                        .ToListAsync();
                    break;
                case (false, false):
                    ViewBag.products = await _work.GenericRepository<Product>().TableNoTracking
                        .Include(x => x.Category)
                        .Where(x => x.Name.Contains(search) || x.Brand.Contains(search) && x.CategoryId == catId)
                        .Skip((page - 1) * 10).Take(10)
                        .ToListAsync();
                    break;
            }

            ViewBag.productsPage = page;

            return View("Product");
        }
        else
        {
            return View("Index");
        }
    }

    public async Task<ActionResult> InsertState(string title)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<State>().AddAsync(new State
            {
                Title = title
            }, CancellationToken.None);
            return RedirectToAction("ManageState");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
    
    public async Task<ActionResult> UpdateState(string title, int id)
    {
        if (User.Identity.IsAuthenticated)
        {
            var state = await _work.GenericRepository<State>().Table.FirstOrDefaultAsync(x => x.Id == id);
            state.Title = title;
            await _work.GenericRepository<State>().UpdateAsync(state, CancellationToken.None);
            return RedirectToAction("ManageState");
        }
        else
        {
            return View("Login");
        }
    }
    public async Task<ActionResult> ManageState(string search, int index)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                ViewBag.States =
                    await _work.GenericRepository<State>().TableNoTracking.Include(x => x.Cities)
                        .OrderByDescending(x => x.Id).ToListAsync();
                return View();
            }
            else
            {
                ViewBag.States =
                    await _work.GenericRepository<State>().TableNoTracking.Include(x => x.Cities)
                        .Where(x => x.Title.Contains(search)).OrderByDescending(x => x.Id).ToListAsync();
                return View();
            }
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
    public async Task<ActionResult> UpdateCat(int id, string title, IFormFile imageCat, bool isActiveCat)
    {
        if (User.Identity.IsAuthenticated)
        {
            Upload up = new Upload(_webHostEnvironment);
            var cat = await _work.GenericRepository<Category>().Table.FirstOrDefaultAsync(x => x.Id == id);
            cat.Name = title;
            await _work.GenericRepository<Category>().UpdateAsync(cat, CancellationToken.None);
            return RedirectToAction("ManageCategory");
        }
        else
        {
            return View("Login");
        }
    }
    public async Task<ActionResult> InsertCat(string title)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<Category>().AddAsync(new Category
            {
                Name = title,
            }, CancellationToken.None);
            return RedirectToAction("ManageCategory");
        }
        else
        {
            return View("Login");
        }
    }

    public async Task<ActionResult> ManageCategory(string search, int index)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking
                    .ToListAsync();
                return View();
            }
            else
            {
                ViewBag.States =
                    ViewBag.Cats = await _work.GenericRepository<Category>().TableNoTracking
                        .Where(x => x.Name.Contains(search)).OrderByDescending(x => x.Id)
                        .ToListAsync();
                return View();
            }
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
    public async Task<ActionResult> UpdateCity(string title, int id, int stateId)
    {
        if (User.Identity.IsAuthenticated)
        {
            var state = await _work.GenericRepository<City>().Table.FirstOrDefaultAsync(x => x.Id == id);
            state.Name = title;
            state.StateId = stateId;
            await _work.GenericRepository<City>().UpdateAsync(state, CancellationToken.None);
            return RedirectToAction("ManageCity");
        }
        else
        {
            return View("Login");
        }
    }
    
    public async Task<ActionResult> ManageCity(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                ViewBag.States =
                    await _work.GenericRepository<State>().TableNoTracking.Include(x => x.Cities)
                        .OrderByDescending(x => x.Id).ToListAsync();
                ViewBag.Cities = await _work.GenericRepository<City>().TableNoTracking.Include(x => x.State)
                    .OrderByDescending(x => x.Id).ToListAsync();
                return View();
            }
            else
            {
                    ViewBag.States =
                        await _work.GenericRepository<State>().TableNoTracking.Include(x => x.Cities)
                            .OrderByDescending(x => x.Id).ToListAsync();
                    ViewBag.Cities = await _work.GenericRepository<City>().TableNoTracking.Include(x => x.State)
                        .Where(x => x.Name.Contains(search) || x.State.Title.Contains(search))
                        .OrderByDescending(x => x.Id).ToListAsync();
                

                return View();
            }
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
    public async Task<ActionResult> InsertCity(string title, int stateId)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<City>().AddAsync(new City
            {
                Name = title,
                StateId = stateId
            }, CancellationToken.None);
            return RedirectToAction("ManageCity");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
    public async Task<ActionResult> UpdatePostMethod(int id, string title, double price)
    {
        if (User.Identity.IsAuthenticated && id > 0)
        {
            var postMethod = await _work.GenericRepository<PostMethod>().Table.FirstOrDefaultAsync(x => x.Id == id);
            postMethod.Title = title;
            postMethod.Price = price;

            await _work.GenericRepository<PostMethod>().UpdateAsync(postMethod, CancellationToken.None);
            return RedirectToAction("ManagePostMethod");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
    public async Task<ActionResult> InsertPostMethod(string title, double price)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<PostMethod>().AddAsync(new PostMethod()
            {
                Title = title,
                Price = price
            }, CancellationToken.None);

            return RedirectToAction("ManagePostMethod");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
    public async Task<ActionResult> ManagePostMethod(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.PostMethod = await _work.GenericRepository<PostMethod>().TableNoTracking
                    .Where(x => x.Title.Contains(search)).OrderByDescending(x => x.Id).ToListAsync();
            }
            else
            {
                ViewBag.PostMethod = await _work.GenericRepository<PostMethod>().TableNoTracking
                    .OrderByDescending(x => x.Id).ToListAsync();
            }

            return View();
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
    public async Task<ActionResult> ManageContact(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.Contact = await _work.GenericRepository<ContactUs>().TableNoTracking
                    .Where(x => x.Name.Contains(search) || x.PhoneNumber.Contains(search) || x.Message.Contains(search))
                    .OrderByDescending(x => x.InsertDate).ToListAsync();
            }
            else
            {
                ViewBag.Contact = await _work.GenericRepository<ContactUs>().TableNoTracking
                    .OrderByDescending(x => x.InsertDate).ToListAsync();
            }

            return View();
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
    public async Task<ActionResult> UpdateDiscount(int id, string code, double amount, int count)
    {
        if (User.Identity.IsAuthenticated)
        {
            var discount = await _work.GenericRepository<DiscountCode>().Table.FirstOrDefaultAsync(x => x.Id == id);
            discount.Count = count;
            discount.Code = code;
            discount.Amount = amount;
            await _work.GenericRepository<DiscountCode>().UpdateAsync(discount, CancellationToken.None);
            return RedirectToAction("ManageDiscount");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
    public async Task<ActionResult> InsertDiscount(string code, double amount, int count)
    {
        if (User.Identity.IsAuthenticated)
        {
            await _work.GenericRepository<DiscountCode>().AddAsync(new DiscountCode()
            {
                Count = count,
                Amount = amount,
                Code = code,
                IsActive = true
            }, CancellationToken.None);
            return RedirectToAction("ManageDiscount");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
    public async Task<ActionResult> ManageDiscount(string search)
    {
        if (User.Identity.IsAuthenticated)
        {
            #region ViewBag

            if (!string.IsNullOrWhiteSpace(search))
            {
                ViewBag.DiscountCode = await _work.GenericRepository<DiscountCode>().TableNoTracking
                    .Where(x => x.Code.Contains(search))
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();
            }
            else
            {
                ViewBag.DiscountCode = await _work.GenericRepository<DiscountCode>().TableNoTracking
                    .OrderByDescending(x => x.Id)
                    .ToListAsync();
            }

            #endregion

            return View();
        }
        else
        {
            return View("Login");
        }
    }
}