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
            if (request.Rating.HasValue)
            {
                if (user.Rating.HasValue && user.RatingCount.HasValue)
                {
                    double totalRating = user.Rating.Value * user.RatingCount.Value;
                    totalRating += request.Rating.Value;

                    user.RatingCount++;
                    user.Rating = totalRating / user.RatingCount.Value;
                }
                else
                {
                    user.Rating = request.Rating.Value;
                    user.RatingCount = 1;
                }
            }

            if (request.Review != null)
            {
                user.Reviews ??= new List<string>();
                user.Reviews.Add(request.Review);
            }
            repositoryFactory.Repository<Profile>().Update(user);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
