using Basket.Api.Data;
using Basket.Api.Models;
using BuildingBlocks.CQRS;
using Discount.Grpc;
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
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient;

        public StoreBasketHandller(IBasketRepository basketRepository
            , DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        {
            _basketRepository = basketRepository;
            _discountProtoServiceClient = discountProtoServiceClient;
        }

        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            // Comunicate With Discount GRPC Service to Calculate TotalPrice of ShoppingCart
            foreach (var item in command.ShoppingCart.Items)
            {
                var discount = await _discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest());
                item.Price = item.Price - discount.Amount;
            }
            var shoppingCart = await _basketRepository.StoreBasket(command.ShoppingCart, cancellationToken);
             return new StoreBasketResult(true, shoppingCart.UserName);
        }
    }
}
