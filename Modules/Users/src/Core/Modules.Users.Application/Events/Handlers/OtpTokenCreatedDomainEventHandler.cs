using Common.Application.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Exceptions;
using Modules.Users.Domain.Events;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Events.Handlers
{
    public class OtpTokenCreatedDomainEventHandler(IUsersDbContext appDbContext) : IDomainEventHandler<TokenCreatedDomainEvent>
    {
        public async Task HandleAsync(TokenCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var token = await appDbContext
                .Tokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(t => t.Id == domainEvent.TokenId, cancellationToken) ?? throw new NotFoundException("Token.NotFound", domainEvent.TokenId);
            if (token.Type == TokenType.Otp || token.Type == TokenType.ForgetPasswordOtp)
            {
            }
        }
    }
}