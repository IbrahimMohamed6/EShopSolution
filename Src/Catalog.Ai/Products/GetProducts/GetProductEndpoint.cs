using Carter;
using Catalog.Ai.Models;
using MediatR;

namespace Catalog.Ai.Products.GetProducts
{
   

    public record GetProductResponse(IEnumerable<Product> Products);
    public class GetProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (ISender sender) =>
            {
                var query = new GetProductQuery();
                var result = await sender.Send(query);
                var response = new GetProductResponse(result.Products);
                return Results.Ok(response);
            });
        }
    }
}
