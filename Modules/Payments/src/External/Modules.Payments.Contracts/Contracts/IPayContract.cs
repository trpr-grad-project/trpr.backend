namespace Modules.Payments.Contracts.Contracts;

public interface IPayContract
{
    public Task Pay(string? refNumber, Guid userId, double balance, string Note);
    public Task Gain(string? refNumber, Guid userId, double balance, string Note);
}
