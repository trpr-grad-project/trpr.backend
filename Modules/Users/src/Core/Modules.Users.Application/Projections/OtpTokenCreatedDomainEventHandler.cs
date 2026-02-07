using Common.Application.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Exceptions;
using Modules.Users.Domain.Events;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Projections
{
    public class OtpTokenCreatedDomainEventHandler(IUsersDbContext usersDbContext) : IDomainEventHandler<TokenCreatedDomainEvent>
    {
        public async Task HandleAsync(TokenCreatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var token = await usersDbContext
                .Tokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(t => t.Id == domainEvent.TokenId, cancellationToken) ?? throw new NotFoundException("Token.NotFound", domainEvent.TokenId);
            if (token.Type == TokenType.Otp || token.Type == TokenType.ForgetPasswordOtp)
            {
            }
        }
    }
}