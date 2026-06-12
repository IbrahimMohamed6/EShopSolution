using Carter;
using Catalog.Ai.Models;
using MediatR;

namespace Catalog.Ai.Products.GetProductByCategory
{

    public record GetProductByCategoryResponse(IEnumerable<Product> Products);
    public class GetProductByCategoryEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string category, ISender sender) =>
            {
                var query = new GetProductByCategoryQuery(category);
                var result = await sender.Send(query);
                var response = new GetProductByCategoryResponse(result.Products);
                return Results.Ok(response);
            });
        }
    }
}
