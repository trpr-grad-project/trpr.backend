namespace Modules.Payments.Application.Dtos
{
    public sealed record BalanceResponse(Guid UserId, double Balance);
}