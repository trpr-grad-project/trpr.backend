using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Application;
using Common.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Mappers;
using Modules.Users.Application.Repositories;
using Modules.Users.Domain.Entities;
using Modules.Users.Domain.ValueObjects;
namespace Modules.Users.Application.Services
{
    public class GuideService(
        RepositoryFactory repositoryFactory, 
        IUnitOfWork unitOfWork, IMapper<DocumentDto, 
        Task<Document>> mapper)
    {
        public async Task<ActionResult<GuideUpgradeResponseDto>> UpgradeToGuide(Guid userId, GuideUpgradeRequestDto request, CancellationToken cancellationToken)
        {
            var user = await repositoryFactory.Repository<User>().GetFirstOrDefaultByFilter(u => u.Id == userId)
                ?? throw new NotFoundException("User.NotFound", userId);
            
            ICollection<Document> docs = [];
            foreach(var doc in request.Documents)
            {
                docs.Append(await mapper.Map(doc));
            }
            GuideUpgradeRequest upgradeRequest = GuideUpgradeRequest.Create(userId, docs);
            repositoryFactory.Repository<GuideUpgradeRequest>().Add(upgradeRequest);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return upgradeRequest.ToResponseDto(request.Documents);
        }
        //public async Task<ActionResult> AllUpgradeRequests(CancellationToken cancellationToken)
        //{
        //    var query = repositoryFactory.Repository<GuideUpgradeRequest>().GetQueryable();
            
        //    var requests = query.Select(x => new
        //    {
        //        x.Id,
        //        x.Subject,
        //        x.Status,
        //        UserResponseDto = x.user.ToResponseDto(),
        //    });
        //}

    }
}
