using Carter;
using Catalog.Ai.Models;
using MediatR;

namespace Catalog.Ai.Products.GetProductById
{
    public record GetProductByIdRequest(Product product);
    public class GetProductByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
            {
                var query = new GetProductByIdQuery(id);
                var result = await sender.Send(query);
                var response = new GetProductByIdRequest(result.Product);
                return Results.Ok(response);
            });
        }
    }
}
