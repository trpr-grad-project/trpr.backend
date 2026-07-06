using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Modules.Users.Contracts.Dtos;
namespace Modules.Users.Contracts.Contracts
{
    public interface IUsersContract
    {
        Task UpdateUserRating(UpdateUserRatingDto request);
    }
}
