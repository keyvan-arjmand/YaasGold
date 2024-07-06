using MediatR;

namespace GoldShop.Application.Admin.V1.Commands.ConfirmCodAdmin;

public record ConfirmCodAdminCommand(string PhoneNumber , string Cod):IRequest;