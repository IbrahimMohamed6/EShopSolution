using Basket.Api.Data;
using Basket.Api.Models;
using BuildingBlocks.CQRS;

namespace Basket.Api._Basket.GetBasket
{

    public record GetBasketQuery(string userName) : IQuery<GetBasketResult>;
    
    public record GetBasketResult(ShoppingCart ShoppingCart);
    public class GetBasketHandller : IQueryHandler<GetBasketQuery, GetBasketResult>
    {
        private readonly IBasketRepository _basketRepository;

        public GetBasketHandller(IBasketRepository basketRepository)
        {
            this._basketRepository = basketRepository;
        }
        public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _basketRepository.GetBasket(request.userName, cancellationToken);
            return new GetBasketResult(shoppingCart);
        }
    }
}
