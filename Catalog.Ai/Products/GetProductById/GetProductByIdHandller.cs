using BuildingBlocks.CQRS;
using Catalog.Ai.CatalogExceptions.ProductExceptions;
using Catalog.Ai.Models;
using Marten;

namespace Catalog.Ai.Products.GetProductById
{
    public record GetProductByIdQuery (Guid Id) : IQuery<GetProductByIdResponse>
    {
        
    }
    public record GetProductByIdResponse(Product Product);
    public class GetProductByIdHandler (IDocumentSession session) : IQueryHandler<GetProductByIdQuery, GetProductByIdResponse>
    {
        public async Task<GetProductByIdResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await session.Query<Product>().FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            if (product == null)
            {
                throw new ProductNotFoundException("product",request.Id);
            }
            return new GetProductByIdResponse(product);
        }
    }
}
