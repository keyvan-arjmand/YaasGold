using GoldShop.Application.Dtos.User;
using MediatR;

namespace GoldShop.Application.Admin.V1.Queries.GetListUser;

public record GetListUserQuery(int Page):IRequest<List<UserDto>>;