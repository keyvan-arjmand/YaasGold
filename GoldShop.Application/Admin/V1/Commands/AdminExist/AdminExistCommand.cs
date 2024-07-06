using GoldShop.Application.Dtos.User;
using MediatR;

namespace GoldShop.Application.Admin.V1.Commands.AdminExist;

public record AdminExistCommand(string PhoneNumber):IRequest<UserLoginDto>;