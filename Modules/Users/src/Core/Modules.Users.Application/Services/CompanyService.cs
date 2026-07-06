using System;
using System.Collections.Generic;
using System.Text;
using Common.Application.Buckets;
using Common.Application.Dtos;
using Common.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Modules.Users.Application.Abstractions;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Application.Dtos.Responses;
using Modules.Users.Application.Mappers;
using Modules.Users.Application.Repositories;
using Modules.Users.Domain.Entities;
using Modules.Users.Domain.Enums;
namespace Modules.Users.Application.Services
{
    public class CompanyService(
        IUnitOfWork unitOfWork,
        RepositoryFactory repositoryFactory,
        AdminUserService adminUserService,
        IFileService fileService
        )
    {
        public async Task<CompanyResponseDto> CreateCompany(CreateCompanyDto dto, CancellationToken cancellationToken = default)
        {
            var userRepository = repositoryFactory.Repository<User>();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var newUser = User.Create(dto.Identifier, dto.Name,"", passwordHash);
            newUser.Verify();
            userRepository.Add(newUser);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            var addrole = await adminUserService.AssignRolesAsync(newUser.Id, new AssignRolesRequestDto { Roles = { Role.Company } } , cancellationToken);
            var companyRepository = repositoryFactory.Repository<Company>();
            var company = Company.Create(newUser.Id, newUser.UserName, newUser.FirstName,dto.Logo, dto.Description);
            companyRepository.Add(company);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return new CompanyResponseDto
            {
                Id = company.Id,
                Identifier = company.Identifier,
                Name = company.Name,
                Description = company.Description,
                Logo = fileService.ResolveUrl(company.Logo),
            };
        }
        public async Task<CompanyResponseDto> GetCompanyById(Guid id, CancellationToken cancellationToken = default)
        {
            var companyRepository = repositoryFactory.Repository<Company>();
            var company = await companyRepository.GetFirstOrDefaultByFilter(x => x.Id == id, includes: q => q.Include(x => x.Guides));
            if (company == null)
            {
                throw new NotFoundException("Company.NotFound");
            }
            return new CompanyResponseDto
            {
                Id = company.Id,
                Identifier = company.Identifier,
                Name = company.Name,
                Description = company.Description,
                Logo = fileService.ResolveUrl(company.Logo),
                Guides = company.Guides.Select(x => x.ToResponseDto()).ToList()
            };
        }

        public async Task<PaginationDto<CompanyResponseDto>> GetAllCompanies(GetCompaniesRequestDto dto, CancellationToken cancellationToken)
        {
            IQueryable<Company> query = repositoryFactory.Repository<Company>().GetQueryable();
            if (dto.Identifier != null)
            {
                query = query.Where(x => x.Identifier.Contains(dto.Identifier));
            }
            if(dto.CompanyName != null)
            {
                query = query.Where(x => x.Name.Contains(dto.CompanyName));
            }
            int TotalItems = await query.CountAsync(cancellationToken);
            List<Company> items = await query.Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize).ToListAsync(cancellationToken);
            return new PaginationDto<CompanyResponseDto>
            {
                Items = items.Select(x => new CompanyResponseDto
                {
                    Id = x.Id,
                    Identifier = x.Identifier,
                    Name = x.Name,
                    Description = x.Description,
                    Logo = fileService.ResolveUrl(x.Logo),
                }).ToList(),
                Page = dto.Page,
                PageSize = dto.PageSize,
                TotalItems = TotalItems
            };
        }

        public async Task<List<UserResponseDto>> GetGuidesBelongingToCompany(Guid companyId, CancellationToken cancellationToken = default)
        {
            var companyRepository = repositoryFactory.Repository<Company>();
            var company = await companyRepository.GetFirstOrDefaultByFilter(x => x.Id == companyId, includes: q => q.Include(x => x.Guides));
            if (company == null)
            {
                throw new NotFoundException("Company.NotFound");
            }
            var guides = company.Guides.Select(x => x.ToResponseDto()).ToList();
            return guides;
        }
        public async Task AddGuideToCompany(Guid companyId, Guid guideId, CancellationToken cancellationToken = default)
        {
            var companyRepository = repositoryFactory.Repository<Company>();
            var userRepository = repositoryFactory.Repository<User>();
            var company = await companyRepository.GetFirstOrDefaultByFilter(x => x.Id == companyId);
            if (company == null)
            {
                throw new NotFoundException("Company.NotFound");
            }
            var guide = await userRepository.GetFirstOrDefaultByFilter(x => x.Id == guideId, includes: q => q.Include(x => x.UserRoles));
            if (guide == null)
            {
                throw new NotFoundException("User.NotFound");
            }
            if (!guide.UserRoles.Any(x => x.Role == Role.Guide))
            {
                throw new BadRequestException("User.NotAGuide");
            }
            company.Guides.Add(guide);
            companyRepository.Update(company);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<UserResponseDto>> GetAllGuides(CancellationToken cancellationToken)
        {
            var userRepository = repositoryFactory.Repository<User>();
            var guides = await userRepository.GetByExpWhereAsync(x => x.UserRoles.Any(r => r.Role == Role.Guide));
            return guides.Select(x => x.ToResponseDto()).ToList();
        }
        
    }
}
