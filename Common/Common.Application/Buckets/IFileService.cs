using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Common.Application.Buckets
{
    public interface IFileService
    {
        public Task<string> UploadFileAsync(IFormFile file);
        public string ResolveUrl(string filePath);
    }
}