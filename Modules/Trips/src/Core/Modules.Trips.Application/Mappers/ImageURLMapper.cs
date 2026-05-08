using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Application;
using Common.Application.Buckets;

namespace Modules.Trips.Application.Mappers
{
    public class ImageURLMapper(IFileService fileService) : IMapper<ICollection<string>, ICollection<string>>
    {
        public ICollection<string> Map(ICollection<string> source)
        {
            ICollection<string> imagePaths = [];
            foreach (var image in source)
            {
                string path = fileService.ResolveUrl(image);
                imagePaths.Add(path);
            }
            return imagePaths;
        }
    }
}
