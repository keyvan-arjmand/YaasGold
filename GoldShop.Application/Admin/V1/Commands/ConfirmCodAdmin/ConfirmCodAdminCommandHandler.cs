using GoldShop.Domain.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoldShop.Application.Admin.V1.Commands.ConfirmCodAdmin;

public class ConfirmCodAdminCommandHandler:IRequestHandler<ConfirmCodAdminCommand,User>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public ConfirmCodAdminCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<User> Handle(ConfirmCodAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.PhoneNumber);
        if (user == null) throw new Exception("User not found");
        if (!user.ConfirmCode.Equals(request.Cod))
        {
            throw new Exception("Code Invalid");
        }
        else if (DateTime.Now >= user.ConfirmCodeExpireTime)
        {
            throw new Exception("Code Expire");
        }
        user.PhoneNumberConfirmed = true;
        await _signInManager.PasswordSignInAsync(user, user.Password, true, false);
        await _userManager.UpdateAsync(user);
        return user;
    }
}