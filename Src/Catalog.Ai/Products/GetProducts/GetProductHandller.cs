using BuildingBlocks.CQRS;
using Catalog.Ai.Models;
using Marten;

namespace Catalog.Ai.Products.GetProducts
{
    public record GetProductQuery
            : IQuery<GetProductResult>;
    public record GetProductResult(IEnumerable<Product> Products);
    public  class GetProductHandller (IDocumentSession session , ILogger<GetProductHandller> logger) : IQueryHandler<GetProductQuery, GetProductResult>
    {
        public async Task<GetProductResult> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var products = await session.Query<Product>().ToListAsync(cancellationToken);
            return new GetProductResult(products);
        }
    }
}
