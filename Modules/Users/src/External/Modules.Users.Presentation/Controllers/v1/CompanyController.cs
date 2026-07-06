using System;
using System.Collections.Generic;
using System.Text;
using Common.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Services;

namespace Modules.Users.Presentation.Controllers.v1
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/v1/company")]
    public class CompanyController(CompanyService companyService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CompanyResponseDto>> CreateCompany(
            [FromBody] CreateCompanyDto dto,
            CancellationToken cancellationToken)
        {
            var company = await companyService.CreateCompany(dto, cancellationToken);
            return Ok(company);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CompanyResponseDto>> GetCompanyById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var company = await companyService.GetCompanyById(id, cancellationToken);
            return Ok(company);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationDto<CompanyResponseDto>>> GetAllCompanies(
            [FromQuery] GetCompaniesRequestDto dto,
            CancellationToken cancellationToken)
        {
            var companies = await companyService.GetAllCompanies(dto, cancellationToken);
            return Ok(companies);
        }

        [HttpGet("{id:guid}/guides")]
        public async Task<ActionResult<List<UserResponseDto>>> GetGuidesBelongingToCompany(
            Guid id,
            CancellationToken cancellationToken)
        {
            var guides = await companyService.GetGuidesBelongingToCompany(id, cancellationToken);
            return Ok(guides);
        }
        [HttpGet("guides")]
        public async Task<ActionResult<List<UserResponseDto>>> GetAllGuides(
            CancellationToken cancellationToken)
        {
            var guides = await companyService.GetAllGuides(cancellationToken);
            return Ok(guides);
        }

        [HttpPost("{id:guid}/guides/{guideId:guid}")]
        public async Task<IActionResult> AddGuideToCompany(
            Guid id,
            Guid guideId,
            CancellationToken cancellationToken)
        {
            await companyService.AddGuideToCompany(id, guideId, cancellationToken);
            return NoContent();
        }
    }
    
}
