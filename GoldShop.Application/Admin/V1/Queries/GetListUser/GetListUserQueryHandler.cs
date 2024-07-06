using GoldShop.Application.Dtos.User;
using GoldShop.Domain.Entity.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GoldShop.Application.Admin.V1.Queries.GetListUser;

public class GetListUserQueryHandler:IRequestHandler<GetListUserQuery,List<UserDto>>
{
    private readonly UserManager<User> _userManager;

    public GetListUserQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<UserDto>> Handle(GetListUserQuery request, CancellationToken cancellationToken)
    {
        return await _userManager.Users.Select(x => new UserDto
        {
            Id = x.Id,
            Name = x.Name,
            InsertDate = x.InsertDate,
            Address = x.Address,
            Family = x.Family,
            ImageName = x.ImageName,
        }).ToListAsync(cancellationToken);
    }
}