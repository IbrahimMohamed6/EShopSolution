using BuildingBlocks.Exeptions;

namespace Catalog.Ai.CatalogExceptions.ProductExceptions
{
    public class ProductNotFoundException: NotFoundException
    {
        public ProductNotFoundException(string name ,object id) :base(name,id)
        {
            
        }
    }
}
