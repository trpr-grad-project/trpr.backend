using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Application.Dtos;
using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Mappers;
using Modules.Users.Application.Repositories;
using Modules.Users.Domain.Entities;
using Modules.Users.Domain.ValueObjects;

namespace Modules.Users.Application.Services;

public class SupportService(
    RepositoryFactory repositoryFactory,
    ILogger<SupportService> logger,
    IUnitOfWork unitOfWork)
{
    public async Task<SupportRequestResponseDto> CreateSupportRequestAsync(
        Guid userId,
        CreateSupportRequestDto createSupportRequestDto,
        CancellationToken cancellationToken = default)
    {
        var supportRequestRepository = repositoryFactory.Repository<SupportRequest>();
        var userRepository = repositoryFactory.Repository<User>();

        var supportRequest = SupportRequest.Create(
            userId,
            createSupportRequestDto.Subject,
            createSupportRequestDto.Description);

        supportRequestRepository.Add(supportRequest);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Support request created with ID: {SupportRequestId} for user: {UserId}",
            supportRequest.Id, userId);

        // Fetch the user to include in response
        var user = await userRepository.GetQueryable()
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        supportRequest.user = user ?? new User();

        return supportRequest.ToResponseDto();
    }

    public async Task<PaginationDto<SupportRequestResponseDto>> GetSupportRequestsAsync(
        GetSupportRequestsRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var supportRequestRepository = repositoryFactory.Repository<SupportRequest>();

        var query = supportRequestRepository.GetQueryable();

        // Filter by status if provided
        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status);
        }

        // Filter by subject search if provided
        if (!string.IsNullOrWhiteSpace(request.SubjectSearch))
        {
            query = query.Where(x => x.Subject.Contains(request.SubjectSearch));
        }

        // Filter by user name search if provided
        if (!string.IsNullOrWhiteSpace(request.NameSearch))
        {
            query = query.Where(x => x.user.FirstName.Contains(request.NameSearch) || x.user.LastName.Contains(request.NameSearch));
        }

        // Order: Unread first, then Read, then by most recent
        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(x => x.user)
            .OrderBy(x => x.Status)
            .ThenByDescending(x => x.CreatedAtUTC)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => x.ToResponseDto())
            .ToListAsync(cancellationToken);

        return PaginationDto<SupportRequestResponseDto>.Create(request.Page, request.PageSize, totalItems, items);
    }

    public async Task<SupportRequestResponseDto> GetSupportRequestByIdAsync(
        Guid supportRequestId,
        CancellationToken cancellationToken = default)
    {
        var supportRequestRepository = repositoryFactory.Repository<SupportRequest>();

        var supportRequest = await supportRequestRepository.GetQueryable()
            .Include(x => x.user)
            .FirstOrDefaultAsync(x => x.Id == supportRequestId, cancellationToken);

        if (supportRequest == null)
        {
            logger.LogWarning("Support request not found with ID: {SupportRequestId}", supportRequestId);
            throw new NotFoundException("SupportRequest.NotFound");
        }

        // Auto-update status to Read when viewed by admin
        if (supportRequest.Status == SupportStatus.Unread)
        {
            supportRequest.Status = SupportStatus.Read;
            supportRequest.UpdatedAtUTC = DateTime.UtcNow;
            supportRequestRepository.Update(supportRequest);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Support request status auto-updated to Read: {SupportRequestId}",
                supportRequestId);
        }

        return supportRequest.ToResponseDto();
    }

    public async Task<List<SupportRequestResponseDto>> GetSupportRequestsByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var supportRequestRepository = repositoryFactory.Repository<SupportRequest>();

        var supportRequests = await supportRequestRepository.GetQueryable()
            .Include(x => x.user)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAtUTC)
            .ToListAsync(cancellationToken);

        return supportRequests.Select(x => x.ToResponseDto()).ToList();
    }
}
