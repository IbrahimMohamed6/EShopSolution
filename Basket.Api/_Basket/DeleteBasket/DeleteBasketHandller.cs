using Basket.Api.Data;
using BuildingBlocks.CQRS;

namespace Basket.Api._Basket.DeleteBasket
{
    public record DeleteBasketRequest(string userName) : ICommand<DeleteBasketResult>;

    public record DeleteBasketResult(bool IsSuccess, string userName);
    public class DeleteBasketHandler : ICommandHandler<DeleteBasketRequest, DeleteBasketResult>
    {
        private readonly IBasketRepository _basketRepository;

        public DeleteBasketHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<DeleteBasketResult> Handle(DeleteBasketRequest request, CancellationToken cancellationToken)
        {
            var isSuccess = await _basketRepository.DeleteBasket(request.userName, cancellationToken);
            return new DeleteBasketResult(isSuccess, request.userName);
        }
    }
}
