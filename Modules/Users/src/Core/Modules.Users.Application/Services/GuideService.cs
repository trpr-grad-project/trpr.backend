using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Repositories;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Services
{
    public class GuideService(IUnitOfWork unitOfWork, 
        IRepository<GuideUpgradeRequest> guideRepository, 
        IRepository<Document> documentRepository)
    {
        public async Task<ActionResult> UpgradeToGuide(Guid userId, GuideUpgradeRequest request)
        {
            var user = 
        }
    }
}
