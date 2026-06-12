using Basket.Api.Models;
using Marten;

namespace Basket.Api.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDocumentSession _session;

        public BasketRepository(IDocumentSession session)
        {
            _session = session;
        }

        public async Task<ShoppingCart?> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var shoppingCart = await _session.Query<ShoppingCart>()
                              .FirstOrDefaultAsync(x => x.UserName == userName, cancellationToken);

            return shoppingCart;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
        {
            var existingCart = await _session.Query<ShoppingCart>()
             .FirstOrDefaultAsync(x => x.UserName == shoppingCart.UserName, cancellationToken);

            if (existingCart is not null)
            {
                existingCart.Items = shoppingCart.Items;
                _session.Update(existingCart);
            }
            else
            {
                _session.Insert(shoppingCart);
            }

            await _session.SaveChangesAsync(cancellationToken);

            return shoppingCart;
        }
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            var existingCart = _session.Query<ShoppingCart>()
                               .FirstOrDefault(x => x.UserName == userName);

            if (existingCart is null)
                return false;

            _session.Delete(existingCart);

            await _session.SaveChangesAsync(cancellationToken);

            return true;
        }

        
    }
}
