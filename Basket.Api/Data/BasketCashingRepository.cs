using Basket.Api.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Data
{
    public class BasketCashingRepository : IBasketRepository
    {
        private readonly IBasketRepository _basketRepository;

        private readonly IDistributedCache _distributedCache;
        public BasketCashingRepository( IBasketRepository basketRepository
            ,IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            _basketRepository = basketRepository;
        }
        

        public async Task<ShoppingCart?> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var Cahse = await _distributedCache.GetStringAsync(userName, cancellationToken);
            if (!string.IsNullOrEmpty(Cahse))
                JsonSerializer.Deserialize<ShoppingCart>(Cahse);

            var basket = await _basketRepository.GetBasket(userName, cancellationToken);
            await _distributedCache.SetStringAsync(userName, JsonSerializer.Serialize<ShoppingCart>(basket), cancellationToken);
            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart shoppingCart, CancellationToken cancellationToken = default)
        {
            await _basketRepository.StoreBasket(shoppingCart, cancellationToken);

            await _distributedCache.SetStringAsync(shoppingCart.UserName, JsonSerializer.Serialize<ShoppingCart>(shoppingCart), cancellationToken);

            return shoppingCart;

        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {

            await _basketRepository.DeleteBasket(userName, cancellationToken);

            await _distributedCache.RemoveAsync(userName, cancellationToken);
            return true;

        }
    }
}
