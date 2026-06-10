using BuildingBlocks.CQRS;
using Catalog.Ai.Models;
using Marten;

namespace Catalog.Ai.Products.GetProductByCategory
{

    public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>
    {

    }
    public record GetProductByCategoryResult(IEnumerable<Product> Products);
    public class GetProductByCategoryHandller(IDocumentSession session) : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
    {
        public Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = session.Query<Product>().Where(p => p.Category.Contains(request.Category)).ToList();
            return Task.FromResult(new GetProductByCategoryResult(products));

        }
    }
}
