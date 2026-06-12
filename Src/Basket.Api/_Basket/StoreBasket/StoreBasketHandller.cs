using Basket.Api.Data;
using Basket.Api.Models;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.Api._Basket.StoreBasket
{

    public record StoreBasketCommand(ShoppingCart ShoppingCart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(bool IsSuccess,string userName);


    public class StoreBasketValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketValidator()
        {
            RuleFor(x => x.ShoppingCart).NotNull().WithMessage("ShoppingCart cannot be null.");
            RuleFor(x => x.ShoppingCart.UserName).NotEmpty().WithMessage("UserName cannot be empty.");
            RuleFor(x => x.ShoppingCart.Items).NotEmpty().WithMessage("Items cannot be empty.");
        }
    }
    public class StoreBasketHandller : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        private readonly IBasketRepository _basketRepository;

        public StoreBasketHandller(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            ShoppingCart cart = command.ShoppingCart;
            var shoppingCart = await _basketRepository.StoreBasket(command.ShoppingCart, cancellationToken);
             return new StoreBasketResult(true, shoppingCart.UserName);
        }
    }
}
