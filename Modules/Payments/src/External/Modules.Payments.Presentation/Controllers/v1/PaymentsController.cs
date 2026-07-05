using Common.Application.Dtos;
using Common.Application.Exceptions;
using Common.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modules.Payments.Application.Abstractions;
using Modules.Payments.Application.Dtos;
using Modules.Payments.Contracts.Contracts;
using Modules.Payments.Domain.Entities;
using Modules.Payments.Domain.ValueObjects;

namespace Modules.Payments.Presentation.Controllers.v1;

[ApiController]
[Route("api/v1/payments")]
[Authorize]
public class PaymentsController(
    IRepository<User> userRepository,
    IPayContract payContract,
    IRepository<Transaction> transactionRepository) : ControllerBase
{
    private Guid UserId => User.GetUserId();
    [HttpPost]
    public async Task<IActionResult> Charge([FromQuery] double ammount)
    {
        await payContract.Gain(null, UserId, ammount, "Charge Account");
        return NoContent();
    }

    [HttpGet("balance")]
    public async Task<ActionResult<BalanceResponse>> GetBalance(CancellationToken cancellationToken)
    {
        var user = await GetUserAsync(UserId);
        return Ok(new BalanceResponse(user.Id, user.Balance));
    }

    [HttpGet("history")]
    public async Task<ActionResult<CursorPageDto<TransactionHistoryResponse, Guid?>>> GetHistory([FromQuery] Guid? cursor = null, [FromQuery] int limit = 30, CancellationToken cancellationToken = default)
    {
        IQueryable<Transaction> query = transactionRepository
            .GetQueryable()
            .Where(x => x.UserID == UserId)
            .OrderByDescending(x => x.Id);

        if (cursor.HasValue)
            query = query.Where(x => x.Id < cursor.Value);

        ICollection<Transaction> transactions =
            await query.Take(limit + 1).ToListAsync(cancellationToken);

        var transactionHistoryResponses = transactions
            .Select(x => new TransactionHistoryResponse(
                x.Id,
                x.RefNumber,
                x.Status,
                x.Balance,
                x.CreatedAtUTC,
                x.Note))
            .ToList();

        Guid? nextCursor = null;
        if (transactionHistoryResponses.Count == (limit + 1))
        {
            transactionHistoryResponses.RemoveAt(limit);
            nextCursor = transactionHistoryResponses.Last().Id;
        }
        var response = new CursorPageDto<TransactionHistoryResponse, Guid?>
        {
            Items = transactionHistoryResponses,
            NextCursor = nextCursor
        };
        return Ok(response);
    }

    private async Task<User> GetUserAsync(Guid userId)
    {
        return await userRepository.GetFirstOrDefaultByFilter(x => x.Id == userId)
        ?? throw new NotFoundException("User.NotFound");
    }
}

