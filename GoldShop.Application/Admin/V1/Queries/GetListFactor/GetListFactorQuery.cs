using GoldShop.Application.Dtos.Factor;
using MediatR;

namespace GoldShop.Application.Admin.V1.Queries.GetListFactor;

public record GetListFactorQuery(int Page):IRequest<List<FactorDto>>;