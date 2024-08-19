using GoldShop.Application.Interfaces;
using GoldShop.Domain.Entity.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoldShop.Controllers;

public class UserController : Controller
{
    private readonly IUnitOfWork _work;
    private readonly UserManager<User> _userManager;

    public UserController(IUnitOfWork work, UserManager<User> userManager)
    {
        _work = work;
        _userManager = userManager;
    }

    public async Task<IActionResult> EditeProfile(string name, string family, string nationalCode,
        string email)
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name) ??
                       new User();
            if (user != null)
            {
                user.Name = name;
                user.Family = family;
                user.Email = email;
                user.NationalCode = nationalCode;
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Profile","Home");
        }
        else
        {
            throw new Exception();
            
        }
    }
    public async Task<IActionResult> UpdateAddress(int id, string name, string address, int stateId, string number,
        string postCode)
    {
        if (User.Identity.IsAuthenticated)
        {
            var result = await _work.GenericRepository<UserAddress>().Table.FirstOrDefaultAsync(x => x.Id == id);
            result.Name = name;
            result.Address = address;
            result.StateId = stateId;
            result.Number = number;
            result.PostCode = postCode;
            await _work.GenericRepository<UserAddress>().UpdateAsync(result, CancellationToken.None);
            return RedirectToAction("Profile","Home");
        }
        else
        {
            return RedirectToAction("Index");
        }
    }
}