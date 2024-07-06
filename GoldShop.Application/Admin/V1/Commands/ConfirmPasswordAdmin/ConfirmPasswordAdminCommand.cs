using MediatR;

namespace GoldShop.Application.Admin.V1.Commands.ConfirmPasswordAdmin;

public record ConfirmPasswordAdminCommand(string Password,string PhoneNumber):IRequest;