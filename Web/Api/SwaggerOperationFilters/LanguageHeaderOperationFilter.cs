using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;


namespace Api.SwaggerOperationFilters
{
    public class LanguageHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= [];

            var alreadyExists = operation.Parameters.Any(p =>
                p.Name == "X-Language" &&
                p.In == ParameterLocation.Header);

            if (alreadyExists)
                return;

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Language",
                In = ParameterLocation.Header,
                Required = false,
                Description = "Select language",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum =
                [
                    new OpenApiString("en"),
                    new OpenApiString("ar")
                ],
                    Default = new OpenApiString("en")
                }
            });
        }
    }
}