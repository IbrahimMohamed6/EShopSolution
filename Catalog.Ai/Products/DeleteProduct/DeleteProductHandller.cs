using BuildingBlocks.CQRS;
using Catalog.Ai.Models;
using Marten;

namespace Catalog.Ai.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>
    {
    }
    public record DeleteProductResult(bool Success, string Message);
    public class DeleteProductHandller(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await session.Query<Product>().FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (product == null)
            {
                return new DeleteProductResult(false, "Product not found");
            }

            session.Delete(product);
            await session.SaveChangesAsync(cancellationToken);
            return new DeleteProductResult(true, "Product deleted successfully");
        }
    }
}
