using System;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ATMService.Filters
{

    /// <summary>
    /// OpenAPI/Swagger example filter for special types
    /// </summary>
    public class ExamplesSchemaFilter : ISchemaFilter
    {

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            schema.Example = GetExampleOrNullFor(context.Type);
        }


        private IOpenApiAny GetExampleOrNullFor(Type type)
        {
            Console.WriteLine(type.Name);

            return type.Name switch
            {
                "Denominations" => new OpenApiObject
                {
                    ["1000"] = new OpenApiInteger(5),
                    ["2000"] = new OpenApiInteger(2),
                    ["5000"] = new OpenApiInteger(1),
                    ["10000"] = new OpenApiInteger(2),
                },
                _ => null,
            };
        }
    }
}
