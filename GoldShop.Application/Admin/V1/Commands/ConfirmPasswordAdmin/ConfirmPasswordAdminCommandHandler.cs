using GoldShop.Domain.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GoldShop.Application.Admin.V1.Commands.ConfirmPasswordAdmin;

public class ConfirmPasswordAdminCommandHandler : IRequestHandler<ConfirmPasswordAdminCommand>
{
    private readonly UserManager<User> _userManager;
    private SignInManager<User> signInManager;

    public ConfirmPasswordAdminCommandHandler(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        this.signInManager = signInManager;
    }

    public async Task Handle(ConfirmPasswordAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.PhoneNumber);
        var userRoles = await _userManager.GetRolesAsync(user);
        if (user == null && userRoles.Any(x => !x.Equals("admin")))
            throw new Exception("User not Exist");
        var result =await signInManager.PasswordSignInAsync(user, request.Password, true, false);
        if(!result.Succeeded)
            throw new Exception("invalid password");
    }
}