using GoldShop.Application.Dtos.User;
using MediatR;

namespace GoldShop.Application.Admin.V1.Commands.LoginCodAdmin;

public record LoginCodAdminCommand(string PhoneNumber):IRequest<UserLoginDto>;