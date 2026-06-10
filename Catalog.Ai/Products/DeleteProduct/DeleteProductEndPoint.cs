using Carter;
using MediatR;

namespace Catalog.Ai.Products.DeleteProduct
{
   
    public record DeleteProductResponse(bool Success, string Message);
    public class DeleteProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/products/{id:guid}", async (Guid id, ISender sender) =>
            {
                var command = new DeleteProductCommand(id);
                var result = await sender.Send(command);
                var response = new DeleteProductResponse(result.Success, result.Message);
                return result.Success ? Results.Ok(response) : Results.BadRequest(response);
            });
        }
    }
}
