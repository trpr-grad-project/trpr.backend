using System;
using System.Collections.Generic;
using System.Text;
using Common.Application.Exceptions;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Repositories;
using Modules.Users.Contracts.Contracts;
using Modules.Users.Contracts.Dtos;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application
{
    public class UpdateUserRatingContract(RepositoryFactory repositoryFactory, IUnitOfWork unitOfWork) : IUsersContract
    {
        public async Task UpdateUserRating(UpdateUserRatingDto request)
        {
            var user = await repositoryFactory.Repository<Profile>().GetFirstOrDefaultByFilter(x => x.Id == request.UserId);
            if (user == null) 
            {
                throw new NotFoundException("User.NotFound");
            }
            user.Rating = request.Rating;
            user.RatingCount = request.RatingCount;

            if (request.Review != null)
            {
                user.Reviews ??= new List<string>();
                user.Reviews.Add(request.Review);
            }
            await unitOfWork.SaveChangesAsync();
        }
    }
}
