using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Application;
using Common.Application.Buckets;
using Microsoft.AspNetCore.Http;
using Modules.Users.Application.Dtos.Requests;
using Modules.Users.Domain.Entities;

namespace Modules.Users.Application.Mappers
{
    public class ImageMapper(IFileService fileService) : IMapper<DocumentDto, Task<Document>>
    {
        public async Task<Document> Map(DocumentDto source)
        {
            var path = await fileService.UploadFileAsync(source.File);
            var doc = new Document()
            {
                FileUrl = path,
                Type = source.Type,
            };
            return doc;
        }
    }
}
