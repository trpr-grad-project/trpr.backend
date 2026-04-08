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
    public class GuideService()
    {
        public Task<ActionResult> UpgradeToGuide(Guid userId, GuideUpgradeRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
