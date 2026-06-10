using Carter;
using Catalog.Ai.CatalogExceptions.ProductExceptions;
using MediatR;

namespace Catalog.Ai.Products.UpdateProduct
{
    public record UpdateProductRequest(Guid Id, string Name, string Description, decimal Price, List<string> Category);

    public record UpdateProductResponse(bool Success, string Message);
    public class UpdateProductEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products/{id:guid}", async (Guid id, UpdateProductRequest request, ISender sender) =>
            {
                if (id != request.Id)
                {
                    throw new ProductNotFoundException("product", id);
                }
                var command = new UpdateProductCommand(request.Id, request.Name, request.Description, request.Price, request.Category);
                var result = await sender.Send(command);
                var response = new UpdateProductResponse(result.Success, result.Message);
                return result.Success ? Results.Ok(response) : Results.BadRequest(response);
            });
        }
    }
}
